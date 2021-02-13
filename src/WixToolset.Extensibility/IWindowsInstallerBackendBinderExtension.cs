// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

namespace WixToolset.Extensibility
{
    using System.Collections.Generic;
    using WixToolset.Data;
    using WixToolset.Data.Symbols;
    using WixToolset.Data.WindowsInstaller;
    using WixToolset.Extensibility.Data;

    /// <summary>
    /// Interface all binder extensions implement.
    /// </summary>
    public interface IWindowsInstallerBackendBinderExtension
    {
        /// <summary>
        /// Table definitions provided by the extension.
        /// </summary>
        IEnumerable<TableDefinition> TableDefinitions { get; }

        /// <summary>
        /// Called before binding occurs.
        /// </summary>
        void PreBackendBind(IBindContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section">The resolved intermedate section.</param>
        void FullyResolved(IntermediateSection section);

        /// <summary>
        /// Finds an existing cabinet that contains the provided files.
        /// </summary>
        /// <param name="cabinetPath">Path to the cabinet.</param>
        /// <param name="files">Files contained in the cabinet.</param>
        /// <returns>Resolved cabinet options or null if the cabinet could not be found.</returns>
        IResolvedCabinet ResolveCabinet(string cabinetPath, IEnumerable<IBindFileWithPath> files);

        /// <summary>
        /// Override layout location for a media.
        /// </summary>
        /// <param name="mediaSymbol">Media symbol.</param>
        /// <param name="mediaLayoutDirectory">Default media layout directory.</param>
        /// <param name="layoutDirectory">Default layout directory.</param>
        /// <returns>Layout location or null to use the default processing.</returns>
        string ResolveMedia(MediaSymbol mediaSymbol, string mediaLayoutDirectory, string layoutDirectory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="symbol"></param>
        /// <param name="output">Windows Installer data </param>
        /// <param name="tableDefinitions">Collection of table definitions available for the output.</param>
        /// <returns>True if the symbol was added to the output, or false if not.</returns>
        bool TryAddSymbolToOutput(IntermediateSection section, IntermediateSymbol symbol, WindowsInstallerData output, TableDefinitionCollection tableDefinitions);

        /// <summary>
        /// Called after all output changes occur and right before the output is bound into its final format.
        /// </summary>
        /// <param name="result">Bind result to process.</param>
        void PostBackendBind(IBindResult result);
    }
}
