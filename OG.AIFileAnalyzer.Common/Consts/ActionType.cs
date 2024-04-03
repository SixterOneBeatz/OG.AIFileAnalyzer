using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OG.AIFileAnalyzer.Common.Consts
{
    /// <summary>
    /// Represents the types of actions in the system.
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Indicates all actions
        /// </summary>
        [Display(Name = "All Actions")]
        All,

        [Display(Name = "Document upload")]
        /// <summary>
        /// Indicates a document upload action.
        /// </summary>
        DocumentUpload,

        [Display(Name = "IA Analysis")]
        /// <summary>
        /// Indicates an action related to artificial intelligence.
        /// </summary>
        IA,

        [Display(Name = "User action")]
        /// <summary>
        /// Indicates a user-related action.
        /// </summary>
        UserAction
    }

}
