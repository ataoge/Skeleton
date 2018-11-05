using System.Collections.Generic;
using Ataoge.GisCore.Geometry;

namespace Ataoge.GisCore.FeatureServer
{
    public class LayerInfo
    {
        public LayerInfo()
        {
            RelationShips = new RelationShip[0];
            AdvancedQueryCapabilities = new AdvancedQueryCapabilities();
        }

        public double CurrentVersion {get; set;}  = 10.5;

        public int Id {get; set;} = 0;
 
        public string Name {get; set;}

        public int ParentLayerId {get; set;} = -1;

        public bool DefaultVisibility {get; set;} = true;

        public double MinScale  {get; set;} = 0;

        public double MaxScacle  {get; set;} = 0;

        public string Type {get; set;} = "Feature Layer";

        public string GeometryType {get;set;} = "esriGeometryPoint";

        public string Description {get;set;} = "";

        public string CopyrightText {get; set;} = "";

        //public string SubTypeField {get; set;} 

        //public string DefaultSubTypeCode {get; set;}

        public EditFieldsInfo EditFieldsInfo {get; set;} = null;

        public OwnershipBasedAccessControlForFeature OwnershipBasedAccessControlForFeature {get;set;} = null;

        public bool SyncCanReturnChanges {get; set;} = true;

        public RelationShip[] RelationShips {get; set;} = null;

        public bool IsDataVersioned {get; set;} = false;

        //public bool IsDataArchived {get; set;}    /Added at 10.6

        //public bool IsCoGoEnabled {get; set;}   /Added at 10.6

        public bool SupportsRollbackOnFailureParameter {get; set;} = true;

        public ArchivingInfo ArchivingInfo {get; set;} = new ArchivingInfo();

        public bool SupportsStatistics {get; set;} = true;

        public bool SupportsAdvancedQueries {get; set;} = true;

        //public double EffectiveMinScale {get; set;} = 0.0;

        //public double EffectiveMaxScale {get; set;} = 0.0;

        public bool SupportsValidateSQL {get; set;} = true;

        public bool SupportsCalculate {get; set;} = false;

        public AdvancedQueryCapabilities AdvancedQueryCapabilities {get;set;} 

        public EsriExtent Extent {get; set;}

        //public HeightModelInfo HeightModelInfo {get; set;} = null; //Added at 10.6. 

        //public HeightModelInfo SourceHeightModelInfo {get; set;} = null; //Added at 10.6. 

        //public SpatialReference SourceSpatialReference {get; set;} //Added at 10.6.

        public DrawingInfo DrawingInfo {get; set;}

        public bool HasM {get; set;} = false;

        public bool HasZ {get; set;} = false;

        //public bool EnableZDefaults {get; set; } = false;

        //public double ZDefault  {get; set;} = 0;

        public bool AllowGeometryUpdtes  {get; set;} = true;

        public bool AllowTrueCurvesUpdates {get; set;} = false;
 
        public bool OnlyAllowTrueCurveUpdateByTrueCurveClients {get; set;} = true;

        //public TimeInfo TimeInfo {get; set;} = null;

        

        public bool HasAttachments {get; set;} = false;

        public bool SupportsApplyEditsWithGlobalIds {get; set;} = true;

        //<esriServerHTMLPopupTypeNone | esriServerHTMLPopupTypeAsURL | esriServerHTMLPopupTypeAsHTMLText>",
        public string HtmlPopupType {get; set;} = "esriServerHTMLPopupTypeAsHTMLText";

        public string ObjectIdField {get; set;} = "objectid";

        public string GlobalIdField {get; set;} = "globalid";

        public string DisplayField {get; set;} = "";

        public string TypeIdField {get; set;} ="";

        public string SubTypeField {get; set;} = "";

        public FieldInfo[] Fields {get; set;}

        public IndexInfo[] Indexes {get; set;} = null;

        public TimeReference DataFieldsTimeReference {get; set;} = null;

        public TypeInfo[] Types {get; set;} = null;

        public TemplateInfo[] Templates {get; set;} = null;

        public SubType[] SubTypes {get; set;} = null;





        public int MaxRecordCount {get; set;} =1000;

        public int StandardMaxRecordCount {get; set;} = 4000;

        public int TileMaxRecordCount {get; set;} = 4000;

        public int MaxRecordCountFactor {get; set;} = 1;


        public bool HasStaticData {get; set;}

        // "JSON, AMF, geoJSON",
        public string SupportedQueryFormats {get; set;}



        //"Create,Delete,Query,Update,Editing"
        public string Capabilities {get; set;}

        public bool UseStandardizeQueries {get; set;}  = true;



  

       

      

   


    }

    public class EditFieldsInfo
    {
        public string CreationDateField {get; set;}

        public string CreatorField {get; set;}

        public string EditDateField {get; set;}

        public string EditorField {get; set;}

        public string Realm {get; set;}
    }

    public class OwnershipBasedAccessControlForFeature
    {
        public bool AllowOthersToUpdate {get; set;}

        public bool AllowOthersToDelete {get; set;}

        public bool AllowOthersToQuery {get; set;}
    }

    public class RelationShip
    {
        public int Id {get; set;}

        public string Name {get; set;}

        public int RelatedTableId {get; set;}

        //"<esriRelCardinalityOneToOne>|<esriRelCardinalityOneToMany>|<esriRelCardinalityManyToMany>";
        public string Cardinality {get; set;}

        //"<esriRelRoleOrigin>|<esriRelRoleDestination>";
        public string Role {get; set;}

        public string KeyField {get;set;}

        public bool Composite {get; set;}

        public int RelationshipTableId {get; set;}

        public string KeyFieldInRelationshipTable {get; set;}

    }

    public class ArchivingInfo
    {
        public bool SupportsQueryWithHistoricMoment {get; set;} = true;

        public long startAcrchivingMoment {get; set;} = 1522748902000; 
    }

    public class HeightModelInfo
    {
        public string HeightModel {get; set;}

        public string VertCRS {get; set;}

        public string HeightUnit {get; set;}
    }

    public class DrawingInfo
    {
        public RendererInfo Renderer {get; set;}
        public double Transparency {get; set;}

        public LabelInfo LabelingInfo {get; set;} = null;
    }

    public class RendererInfo
    {
        public string Type {get; set;}

        //public string Field1 {get; set;}

        //public string Field2 {get; set;}

        //public string Field3 {get;set;}

        public ISymbolInfo Symbol {get; set;}


       public string Label {get; set; } = "";

       public string Description {get; set;} = "";
    }

    public class LabelInfo
    {
        
    }

    public interface ISymbolInfo
    {

    }

    public class BaseSymbolInfo : ISymbolInfo
    {
        public string Type {get;set;}

        public string Style {get; set;}



        public int[] Color {get; set;}

        public Outline Outline {get; set;} 
    }

    public class LineSymbolInfo : ISymbolInfo
    {
        public string Type {get;set;}

        public string Style {get; set;}



        public int[] Color {get; set;}

        public double Width {get; set;} = 1;
    }

    public class SymbolInfo : ISymbolInfo
    {
        public string Type {get;set;}

        public string Style {get; set;}



        public int[] Color {get; set;}

        public int Size {get; set;}

        public double Angle {get; set;} = 0;

        public double XOffset {get; set;} = 0;

        public double YOffset {get; set;} = 0;

        public Outline Outline {get; set;} 
    }

    

    public class Outline
    {
        public int[] Color {get; set;}

        public double Width {get; set;} = 1;
    }

    public class FullOutline : Outline
    {
        public string Type {get;set;}

        public string Style {get; set;}

    }

    public class TimeInfo
    {
        public string StartTimeField {get; set;}

        public string EndTimeField {get; set;}

        public string TrackIdField {get; set;}
    
        public string[] TimeExtent {get; set;}

        public TimeReference TimeReference {get; set;}

        public long TimeInterval {get; set;}

        public string TimeIntervalUnits {get; set;}
    }

    public class TimeReference
    {
        public string TimeZone {get; set;}  = "UTC";

        public bool RespectsDaylightSaving {get; set;} = false;
    }

    public class AdvancedQueryCapabilities
    {
        public bool SupportsPagination {get; set;} = true;

        public bool SupportsTrueCurve {get; set;} = true;

        public bool SupportsQueryWithDistance {get; set;} = true;

        public bool SupportsReturningQueryExtent {get; set;} = true;

        public bool SupportsStatistics {get;set;} = true;

        public bool SupportsOrderBy {get; set;} = true;

        public bool SupportsDistinct {get; set;} = true;

       

      

        



    }

   

    public class FieldDomain
    {
        public string Type {get; set;}

        public string Name {get; set;}

        public string MergePolicy {get; set;} = "esriMPTDefaultValue";
        public string SplitPolicy {get; set; } = "esriSPTDefaultValue";
    }

    public class TypeInfo
    {
        public int Id {get; set;}

        public string Name {get; set;}

        public DomainInfo Domains {get; set;}

        
    }

    public class DomainInfo
    {
        public string Description {get; set;}
    }

    public class TemplateInfo
    {
        public string Name {get; set;}

        public string Description {get; set;}

        public ProtoType protoType {get; set;}

        //"esriFeatureEditToolNone | esriFeatureEditToolPoint | esriFeatureEditToolLine | esriFeatureEditToolPolygon |                        esriFeatureEditToolAutoCompletePolygon | esriFeatureEditToolCircle | esriFeatureEditToolEllipse | esriFeatureEditToolRectangle |                        esriFeatureEditToolFreehand"
        public string DrawingInfo {get; set;}
    }

    public class ProtoType
    {
        IDictionary<string, object> Attributes {get ;set;}
    }

    public class SubType
    {
        public string Code {get; set;}

        public string Name {get; set;}

        public DefalutValues DefalutValues {get; set;}

        public DomainInfo Domains {get; set;}
    }

    public class DefalutValues
    {
        public object FieldName1 {get; set;}

        public object FieldName2 {get; set;}
    }

    public class IndexInfo
    {
        public string Name {get; set;}

        public string Fields {get; set;}

        public bool IsAscending {get; set;} = true;

        public bool IsUnique {get;set;} = false;

        public string Description {get; set;} = "";

    }
}