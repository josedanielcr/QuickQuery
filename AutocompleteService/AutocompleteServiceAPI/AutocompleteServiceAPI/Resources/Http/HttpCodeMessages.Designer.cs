﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutocompleteServiceAPI.Resources.Http {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class HttpCodeMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal HttpCodeMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AutocompleteServiceAPI.Resources.Http.HttpCodeMessages", typeof(HttpCodeMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The executed delete endpoint {0} returned {1}.
        /// </summary>
        public static string Http_DeleteError {
            get {
                return ResourceManager.GetString("Http.DeleteError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The executed get endpoint {0} returned {1}.
        /// </summary>
        public static string Http_GetError {
            get {
                return ResourceManager.GetString("Http.GetError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The executed post endpoint {0} returned {1}.
        /// </summary>
        public static string Http_PostError {
            get {
                return ResourceManager.GetString("Http.PostError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The executed put endpoint {0} returned {1}.
        /// </summary>
        public static string Http_PutError {
            get {
                return ResourceManager.GetString("Http.PutError", resourceCulture);
            }
        }
    }
}
