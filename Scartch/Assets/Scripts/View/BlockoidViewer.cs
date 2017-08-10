namespace View
{
    public abstract class BlockoidViewer : Resources.ScriptingElementViewer
    {
        public abstract BlockViewer Next { get; set; }
        public abstract void Highlight(bool v);
    }
}