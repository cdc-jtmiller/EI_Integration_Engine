using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
using NHapi.Base;
using NHapi.Base.Parser;
using NHapi.Base.Model;
using NHapi.Base.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;
using Newtonsoft.Json.Serialization;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Reflection.Metadata;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Serialization.ObjectGraphVisitors;


namespace ParseHL7
{
    public partial class frmParseHL7 : Form
    {
        // Firely public server
        private const string _fhirServer = "http://vonk.fire.ly";

        // List to collect and manage error messages
        private List<string> errorCollection = new List<string>();
        // Current segment being processed
        private string currentSegment = string.Empty;
        // Current field number being processed
        private int fieldNumber = 0;
        // List of CSegment to manage segment data
        private List<CSegment> segments = new List<CSegment>();


        public frmParseHL7()
        {
            InitializeComponent();
        }

        private void btnBrowseHL7_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the initial directory (optional)
                string msgPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\")) + @"messages\";
                //openFileDialog.InitialDirectory = @"C:\Temp\HL7\Messages\GenV2";
                openFileDialog.InitialDirectory = msgPath;
                openFileDialog.RestoreDirectory = true;

                // Set the title of the dialog (optional)
                openFileDialog.Title = "Open HL7 File";

                // Set the filter to restrict file types (optional)
                openFileDialog.Filter = "Text files (*.hl7)|*.hl7|All files (*.*)|*.*";

                // Open the dialog and check if the user selected a file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path
                    string filePath = openFileDialog.FileName;

                    // Display the file path in the text box
                    tbHL7Path.Text = filePath;
                    execCode();
                }
            }
        }

        private void execCode()
        {
            if (rtbRaw.Text != String.Empty)
            {
                rtbRaw.SelectAll();
                rtbRaw.Clear();

                rtbXML.SelectAll();
                rtbXML.Clear();

                rtbXml2Json.SelectAll();
                rtbXml2Json.Clear();

                rtbFHIR.SelectAll();
                rtbFHIR.Clear();

                rtbYaml.SelectAll();
                rtbYaml.Clear();
            }

            string filePath = tbHL7Path.Text;
            if (!string.IsNullOrEmpty(filePath))
            {
                string hl7Content = File.ReadAllText(filePath);
                DisplayRAW(hl7Content);

                try
                {
                    var parser = new PipeParser();
                    IMessage parsedMessage = parser.Parse(hl7Content.Trim());

                    if (parsedMessage != null)
                    {
                        var root = new XElement("HL7Message");
                        LoopOnStructures(parsedMessage, root);

                        XElement xmlElement = DisplayXML(root);
                        DisplayJSONxXML(xmlElement);

                        GenerateFHIRMessage(parsedMessage);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("You must enter a valid path.");
            }

        }

        private void LoopOnStructures(IStructure structure, XElement parentElement)
        {
            if (structure is ISegment segment)
            {
                var segmentElement = new XElement(segment.GetStructureName());
                parentElement.Add(segmentElement);

                for (int i = 1; i <= segment.NumFields(); i++)
                {
                    var field = segment.GetField(i);
                    var fieldElement = new XElement($"Field{i}");
                    segmentElement.Add(fieldElement);

                    ExtractFieldData(fieldElement, field);
                }
            }
            else if (structure is IGroup group)
            {
                var groupElement = new XElement(group.GetStructureName());
                parentElement.Add(groupElement);

                foreach (var name in group.Names)
                {
                    var children = group.GetAll(name);
                    foreach (var child in children)
                    {
                        LoopOnStructures(child, groupElement);
                    }
                }
            }
        }

        private void LoopOnFields(IType[] types, int level = 0)
        {
            foreach (IType type in types)
            {
                //Field could be composite or primitive
                if (type is IComposite compositeItem)
                {
                    LoopOnFields(compositeItem.Components.ToArray(), level + 1);
                }
                else
                {
                    //Here we can find structural issues.
                    if (type is IPrimitive primitive)
                    {
                        int extraComponentCount = primitive.ExtraComponents.numComponents();
                        if (extraComponentCount > 0)
                        {
                            CSegment _cseg = segments.FirstOrDefault(cseg => cseg.Name.Equals(currentSegment));
                            if (_cseg != null)
                            {
                                errorCollection.Add(string.Format("{0}^{1}^{2}^102&Data type error&HL7nnnn", currentSegment, _cseg.Sequence, fieldNumber));
                            }
                        }
                    }
                }
            }
        }

        private void LoopOnFields(IType[] types)
        {
            foreach (IType type in types)
            {
                //Field could be composite or primitive
                IComposite compositeItem = type as IComposite;
                if (compositeItem != null)
                    LoopOnFields((IType[]) compositeItem.Components);

                //Here we can find structural issues.
                if (type.ExtraComponents.numComponents() > 0)
                {
                    CSegment _cseg = segments.Where(cseg => cseg.Name.Equals(currentSegment)).First<CSegment>();
                    errorCollection.Add(string.Format("{0}^{1}^{2}^102&Data type error&HL7nnnn", currentSegment, _cseg.Sequence, fieldNumber));
                }
            }
        }

        public static string ConvertXmlToJson(XElement xmlElement)
        {
            try
            {
                string json = JsonConvert.SerializeXNode(xmlElement);

                // Optional: Format the JSON string for better readability
                JToken parsedJson = JToken.Parse(json);
                string formattedJson = parsedJson.ToString(Formatting.Indented);

                return formattedJson;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the conversion
                Console.WriteLine($"Error converting XML to JSON: {ex.Message}");
                return null;
            }
        }

        private void DisplayRAW(string messageText)
        {
            rtbRaw.Text = messageText;
        }

        private XElement DisplayXML(XElement root)
        {
            string xmlContent = root.ToString();
            rtbXML.Text = xmlContent;

            // Generate YAML from XML
            GenerateYAMLFromXML(root);

            return root;
        }

        private void DisplayJSONxXML(XElement root)
        {
            string jsonContent = ConvertXmlToJson(root);
            rtbXml2Json.Text = jsonContent;
        }

        private void ExtractFieldData(XElement fieldElement, IType[] fields)
        {
            foreach (IType field in fields)
            {
                if (field is IComposite composite)
                {
                    // Process composite fields recursively
                    foreach (var component in composite.Components)
                    {
                        var componentElement = new XElement("Component");
                        ExtractFieldData(componentElement, new[] { component });
                        if (componentElement.HasElements || !string.IsNullOrEmpty(componentElement.Value))
                        {
                            fieldElement.Add(componentElement);
                        }
                    }
                }
                else if (field is IPrimitive primitive)
                {
                    // Extract the value from primitive fields
                    string value = primitive.Value?.ToString() ?? string.Empty;
                    fieldElement.Value = value;
                }
                else if (field is Varies varies)
                {
                    // Handle Varies type
                    IType data = varies.Data;
                    if (data is IPrimitive primitiveData)
                    {
                        string value = primitiveData.Value?.ToString() ?? string.Empty;
                        fieldElement.Value = value;
                    }
                    else if (data is IComposite compositeData)
                    {
                        // Recursive data extraction for composite data within Varies type
                        foreach (var component in compositeData.Components)
                        {
                            var componentElement = new XElement("Component");
                            ExtractFieldData(componentElement, new[] { component });
                            if (componentElement.HasElements || !string.IsNullOrEmpty(componentElement.Value))
                            {
                                fieldElement.Add(componentElement);
                            }
                        }
                    }
                    else
                    {
                        fieldElement.Value = data?.ToString() ?? string.Empty;
                    }
                }
                else
                {
                    fieldElement.Value = field?.ToString() ?? string.Empty;
                }
            }
        }

        private void GenerateFHIRMessage(IMessage parsedMessage)
        {
            try
            {
                // Use a list to collect JSON objects
                var fhirJsonList = new List<JObject>();

                // Traverse the HL7 message structures and build the JSON objects
                LoopOnStructuresFHIR(parsedMessage, fhirJsonList);

                // Serialize the list of JSON objects to a JSON array
                var fhirJsonArray = JArray.FromObject(fhirJsonList);

                // Format the JSON output
                var formattedJson = fhirJsonArray.ToString(Formatting.Indented);

                // Display the JSON output in the rtbFHIR rich text box
                rtbFHIR.Text = formattedJson;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating FHIR message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoopOnStructuresFHIR(IStructure structure, List<JObject> fhirJsonList)
        {
            if (structure is ISegment segment)
            {
                var segmentName = segment.GetStructureName();

                for (int i = 1; i <= segment.NumFields(); i++)
                {
                    var field = segment.GetField(i);
                    foreach (var fieldValue in field)
                    {
                        var extractedValue = ExtractFieldValue(fieldValue);
                        if (!string.IsNullOrEmpty(extractedValue))
                        {
                            // Construct the key based on the segment name and field number
                            var key = $"{segmentName}_Field{i}";

                            // Create the JSON object with the key-value pair
                            var jsonObject = new JObject
                    {
                        { key, extractedValue }
                    };

                            fhirJsonList.Add(jsonObject);
                        }
                    }
                }
            }
            else if (structure is IGroup group)
            {
                foreach (var name in group.Names)
                {
                    var children = group.GetAll(name);
                    foreach (var child in children)
                    {
                        LoopOnStructuresFHIR(child, fhirJsonList);
                    }
                }
            }
        }

        private string ExtractFieldValue(IType field)
        {
            if (field is AbstractPrimitive primitive)
            {
                return primitive.Value;
            }
            else if (field is IComposite composite)
            {
                var componentValues = composite.Components.Select(ExtractFieldValue).Where(val => !string.IsNullOrEmpty(val));
                return string.Join("^", componentValues);
            }
            return null;
        }

        private DomainResource CreateFhirResource(string segmentName)
        {
            switch (segmentName)
            {
                case "MSH":
                    return new MessageHeader();
                case "EVN":
                    return new MessageHeader();
                case "PID":
                    return new Patient();
                case "PD1":
                    return new Patient();
                case "NK1":
                    return new RelatedPerson();
                case "PV1":
                    return new Encounter();
                case "PV2":
                    return new Encounter();
                case "IN1":
                    return new Coverage();
                case "IN2":
                    return new Coverage();
                case "IN3":
                    return new Coverage();
                case "GT1":
                    return new RelatedPerson();
                case "AL1":
                    return new AllergyIntolerance();
                case "ORC":
                    return new ServiceRequest();
                case "OBR":
                    return new ServiceRequest();
                case "OBX":
                    return new Observation();
                case "NTE":
                    return new Basic
                    {
                        Code = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://hl7.org/fhir/StructureDefinition/NTE",
                                    Code = "NTE",
                                    Display = "Notes and Comments"
                                }
                            }
                        }
                    };

                case "DG1":
                    return new Condition();
                case "PR1":
                    return new Procedure();
                case "ROL":
                    return new PractitionerRole();
                case "ARV":
                    return new Consent();
                case "DRG":
                    return new ExplanationOfBenefit();
                case "FT1":
                    return new ExplanationOfBenefit();
                case "CTI":
                    return new ResearchStudy();
                default:
                    throw new ArgumentException($"Unsupported HL7 segment type: {segmentName}");
            }
        }

        private void GenerateYAMLFromXML(XElement xmlElement)
        {
            try
            {
                // Create a custom YAML serializer with formatting options and recursion handling
                var serializer = new YamlDotNet.Serialization.SerializerBuilder()
            .WithMaximumRecursion(100) // Set the maximum recursion depth
            .DisableAliases() // Disable alias emission to handle circular references
            .Build();

                // Convert XML to a simplified object graph
                var objectGraph = SimplifyXElementGraph(xmlElement);

                // Convert the simplified object graph to YAML
                var yamlOutput = serializer.Serialize(objectGraph);

                // Display the YAML output in the rtbYaml rich text box
                rtbYaml.Text = yamlOutput;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating YAML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private object SimplifyXElementGraph(XElement element)
        {
            if (element.HasElements)
            {
                var simplifiedElements = new Dictionary<string, object>();
                foreach (var childElement in element.Elements())
                {
                    var childElementName = childElement.Name.LocalName;
                    if (simplifiedElements.ContainsKey(childElementName))
                    {
                        if (simplifiedElements[childElementName] is List<object> list)
                        {
                            list.Add(SimplifyXElementGraph(childElement));
                        }
                        else
                        {
                            var existingValue = simplifiedElements[childElementName];
                            simplifiedElements[childElementName] = new List<object> { existingValue, SimplifyXElementGraph(childElement) };
                        }
                    }
                    else
                    {
                        simplifiedElements[childElementName] = SimplifyXElementGraph(childElement);
                    }
                }
                return simplifiedElements;
            }
            else
            {
                return element.Value;
            }
        }

        class CSegment
        {
            public string Name
            {
                get;
                set;
            }

            public int Sequence
            {
                get;
                set;
            }
        }
    }
}