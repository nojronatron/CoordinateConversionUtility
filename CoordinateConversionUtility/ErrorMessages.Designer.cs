﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoordinateConversionUtility {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CoordinateConversionUtility.ErrorMessages", typeof(ErrorMessages).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Expected DD Coordinate format is: D.dddd*[N/S],D.dddd*[E/W]&quot;.
        /// </summary>
        internal static string ddCoordinatesArgumentNull {
            get {
                return ResourceManager.GetString("ddCoordinatesArgumentNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Expected DDM Coordinate format is: D*m.m&apos;[N/S],D*m.m&apos;[E/W]&quot;.
        /// </summary>
        internal static string ddmCoordinatesInputError {
            get {
                return ResourceManager.GetString("ddmCoordinatesInputError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Expected DMS Coordinate format is: DD*MM&apos;SS\&quot;[N/S],DDD*MM&apos;SS\&quot;[E/W]&quot;.
        /// </summary>
        internal static string dmsCoordinatesArgumentNull {
            get {
                return ResourceManager.GetString("dmsCoordinatesArgumentNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;gridsquare is a six-character string in the format AA##aa.&quot;.
        /// </summary>
        internal static string gridsquareArgumentOutOfRange {
            get {
                return ResourceManager.GetString("gridsquareArgumentOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Table lookup failed due to incorrect lookup value&quot;.
        /// </summary>
        internal static string gridsquareIndexOutOfRange {
            get {
                return ResourceManager.GetString("gridsquareIndexOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Table Generation Failed. See inner exception.&quot;.
        /// </summary>
        internal static string tableGenerationFailed {
            get {
                return ResourceManager.GetString("tableGenerationFailed", resourceCulture);
            }
        }
    }
}
