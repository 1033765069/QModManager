﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace QModManager
{
    public static class Utility
    {
        /// <summary>
        /// Try to resolve and load the given assembly DLL.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly, of the type <see cref="AssemblyName"/>.</param>
        /// <param name="directory">Directory to search the assembly from.</param>
        /// <param name="assembly">The loaded assembly.</param>
        /// <returns>True, if the assembly was found and loaded. Otherwise, false.</returns>
        private static bool TryResolveDllAssembly<T>(AssemblyName assemblyName, string directory, Func<string, T> loader, out T assembly) where T : class
        {
            assembly = null;

            var potentialDirectories = new List<string> { directory };

            potentialDirectories.AddRange(Directory.GetDirectories(directory, "*", SearchOption.AllDirectories));

            foreach (string subDirectory in potentialDirectories)
            {
                string path = Path.Combine(subDirectory, $"{assemblyName.Name}.dll");

                if (!File.Exists(path))
                    continue;

                try
                {
                    assembly = loader(path);
                }
                catch (Exception)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Try to resolve and load the given assembly DLL.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly, of the type <see cref="AssemblyName"/>.</param>
        /// <param name="directory">Directory to search the assembly from.</param>
        /// <param name="assembly">The loaded assembly.</param>
        /// <returns>True, if the assembly was found and loaded. Otherwise, false.</returns>
        public static bool TryResolveDllAssembly(AssemblyName assemblyName, string directory, out Assembly assembly)
        {
            return TryResolveDllAssembly(assemblyName, directory, Assembly.LoadFile, out assembly);
        }
    }
}
