namespace Ataoge.GisCore.FeatureServer
{

    public class LayersQueryParams :BaseQueryParams
    {
        //layerId1:layerDef1;layerId2:layerDef2 where 
        //{ "<layerId1>" : "<layerDef1>" , "<layerId2>" : "<layerDef2>" }
        //[{ "layerId" : <layerId1>,"where": "<where clause>", "outfields": "<field1>,<field2>"},{"layerId" : <layerId2>,"where": "<where clause>", "outfields": "<field1>,<field2>"}]
        public string LayerDefs {get; set;}
    }
    public class LayerQueryParams : BaseQueryParams
    {
        
        public string Where {get; set;}

        public string ObjectIds {get; set;}

        public string RelationParram {get; set;}

        public double Distance {get; set;}

        public string Units {get; set;}

        public string OutFields {get; set;}

        public bool ReturnDistinctValues {get; set;}

        public bool ReturnExtentOnly {get; set;}

        public string OrderByFields {get; set;}

        public string GroupByFieldsForStatics {get; set;}

        //Json Array
        public string OutStatistics {get; set;}

        public string MultiPatchOPtion {get; set;}

        public int ResultOffset {get; set;}
       
        public int ResultRecordCount {get; set;}

        //json
        public string QuantizationParameters {get; set;}

        public bool ReturnCentroid {get; set;}

        //Values: none | standard | tile
        public string ResultType {get; set;}

     
        public bool ReturnExceededLimitFeature {get; set;}









    }
}