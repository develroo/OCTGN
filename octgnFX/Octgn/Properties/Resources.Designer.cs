﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Octgn.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Octgn.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to class idict: 
        ///    def __init__(self,inDict=None): 
        ///        &quot;&quot;&quot;Constructor: takes conventional dictionary 
        ///           as input (or nothing)&quot;&quot;&quot; 
        ///        self.dict = {} 
        ///        if inDict != None: 
        ///            for key in inDict: 
        ///                k = key.lower() 
        ///                self.dict[k] = (key, inDict[key]) 
        ///        self.keyList = self.dict.keys() 
        ///        return 
        /// 
        ///    def __iter__(self): 
        ///        self.iterPosition = 0 
        ///        return(self) 
        /// 
        ///    def next(self): 
        ///        if self.iterPosi [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CaseInsensitiveDict {
            get {
                return ResourceManager.GetString("CaseInsensitiveDict", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to class NamedObject(object):
        ///  def __init__(self, id, name):
        ///    self._id = id
        ///    self._name = name
        ///  @property
        ///  def name(self): return self._name
        ///  
        ///class Group(NamedObject):
        ///  def __init__(self, id, name, player = None):
        ///    NamedObject.__init__(self, id, name)
        ///    self._player = player
        ///  @property
        ///  def player(self): return self._player
        ///
        ///class Counter(NamedObject):
        ///  def __init__(self, id, name, player):
        ///    NamedObject.__init__(self, id, name)
        ///    self._player = player
        ///  @property
        ///  d [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PythonAPI {
            get {
                return ResourceManager.GetString("PythonAPI", resourceCulture);
            }
        }
    }
}
