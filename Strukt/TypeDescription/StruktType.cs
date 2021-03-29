namespace Strukt.TypeDescription
{
    public class StruktType
    {
        public int TypeId;
        public int SizeOf;
        public string Name;
        public string PackageName;
        public bool NativeType;
    }
    
    internal class ArgumentInfo
    {
        public string Name { get; set; }
        public StruktType TypeName;
    }
}