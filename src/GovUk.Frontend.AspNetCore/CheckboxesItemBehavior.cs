#nullable enable

namespace GovUk.Frontend.AspNetCore
{
    /// <summary>
    /// Defines the selection behavior options for a checkbox item.
    /// </summary>
    public enum CheckboxesItemBehavior
    {
        /// <summary>
        /// The default behaviour.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Indicates that when this item is checked then no other items should be checked.
        /// </summary>
        Exclusive = 1
    }
}
