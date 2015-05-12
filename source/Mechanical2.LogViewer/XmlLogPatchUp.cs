using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mechanical.LogViewer
{
    public static class XmlLogPatchUp
    {
        public static string CloseOpenTags( string xmlPath )
        {
            //// NOTE: If the app is terminated without properly shutting down, then
            ////       XML tags will need to be closed. Unless this happened in the middle
            ////       of writing a log entry, no data should have been lost.

            var sb = new StringBuilder();
            using( var writer = XmlWriter.Create(sb) )
            {
                var openElements = new List<string>();
                using( var reader = XmlReader.Create(xmlPath) )
                {
                    try
                    {
                        while( reader.Read() )
                        {
                            switch( reader.NodeType )
                            {
                            case XmlNodeType.Element:
                                writer.WriteStartElement(reader.Name);

                                if( reader.IsEmptyElement ) // no EndElement node in this case!
                                    writer.WriteEndElement();
                                else
                                    openElements.Add(reader.Name);
                                break;

                            case XmlNodeType.Text:
                                writer.WriteString(reader.Value);
                                break;

                            case XmlNodeType.EndElement:
                                openElements.RemoveAt(openElements.Count - 1);
                                writer.WriteFullEndElement();
                                break;
                            }
                        }
                    }
                    catch( System.Xml.XmlException )
                    {
                        //// Unexpected end of file found by Read()
                    }
                }

                while( openElements.Count != 0 )
                {
                    openElements.RemoveAt(openElements.Count - 1);
                    writer.WriteFullEndElement();
                }
            }

            return sb.ToString();
        }
    }
}
