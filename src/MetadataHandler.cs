using System;
using System.Collections.Generic;
using Landis.Library.Metadata;
using Landis.Core;
using System.IO;
using Landis.Utilities;

namespace Landis.Extension.Output.MaxSpeciesAge
{
    public static class MetadataHandler
    {

        public static ExtensionMetadata Extension { get; set; }
        public static void InitializeMetadata(string speciesNameTemplate, IEnumerable<ISpecies> selectedSpecies)
        {

            ScenarioReplicationMetadata scenRep = new ScenarioReplicationMetadata()
            {
                RasterOutCellArea = PlugIn.ModelCore.CellArea,
                TimeMin = PlugIn.ModelCore.StartTime,
                TimeMax = PlugIn.ModelCore.EndTime,
            };

            Extension = new ExtensionMetadata(PlugIn.ModelCore)
            {
                Name = PlugIn.ExtensionName,
                TimeInterval = PlugIn.ModelCore.CurrentTime,
                ScenarioReplicationMetadata = scenRep
            };

            //---------------------------------------            
            //          map outputs:         
            //---------------------------------------

            foreach(ISpecies species in selectedSpecies)
            {
                OutputMetadata mapOut_max = new OutputMetadata()
                {
                    Type = OutputType.Map,
                    Name = "max_age_" + species.Name,
                    FilePath = MapNameTemplates.ReplaceTemplateVars(speciesNameTemplate, species.Name, PlugIn.ModelCore.CurrentTime),
                    Map_DataType = MapDataType.Continuous,
                    Visualize = true,
                };
                Extension.OutputMetadatas.Add(mapOut_max);
            }

            OutputMetadata mapOut_max_all = new OutputMetadata()
            {
                Type = OutputType.Map,
                Name = "AllSppMaxAge",
                FilePath = MapNameTemplates.ReplaceTemplateVars(speciesNameTemplate, "AllSppMaxAge", PlugIn.ModelCore.CurrentTime),
                Map_DataType = MapDataType.Continuous,
                Visualize = true,
            };
            Extension.OutputMetadatas.Add(mapOut_max_all);



            //---------------------------------------
            MetadataProvider mp = new MetadataProvider(Extension);
            mp.WriteMetadataToXMLFile("Metadata", Extension.Name, Extension.Name);
        }
        public static void CreateDirectory(string path)
        {
            path = path.Trim(null);
            if (path.Length == 0)
                throw new ArgumentException("path is empty or just whitespace");

            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
            {
                Landis.Utilities.Directory.EnsureExists(dir);
            }

            return;
        }
    }
}
