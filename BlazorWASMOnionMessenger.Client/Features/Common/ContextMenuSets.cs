using Radzen;

namespace BlazorWASMOnionMessenger.Client.Features.Common
{
    public class ContextMenuSets
    {
        public static List<ContextMenuItem> deleteOnly { get; } = new List<ContextMenuItem> {
            new ContextMenuItem(){ Text = "Delete", Value = "delete", Icon = "delete" }};

        public static List<ContextMenuItem> deleteAndEdit { get; } = new List<ContextMenuItem> {
            new ContextMenuItem(){ Text = "Delete", Value = "delete", Icon = "delete" },
            new ContextMenuItem(){ Text = "Edit", Value = "edit", Icon = "build"}};
    }
}
