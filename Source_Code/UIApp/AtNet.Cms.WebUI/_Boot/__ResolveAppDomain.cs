﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using AtNet.Cms.Conf;

namespace AtNet.Cms
{
    internal class __ResolveAppDomain
    {
        public static void Resolve()
        {
            AppDomain domain = AppDomain.CurrentDomain;
            string directory = domain.BaseDirectory;
            string resolveDirName = CmsVariables.FRAMEWORK_ASSEMBLY_PATH;
            //
            //设为隐藏目录
            //
			domain.AssemblyResolve+= delegate(object sender, ResolveEventArgs args) 
			{
				
				string filePath=String.Concat(directory,resolveDirName,args.Name.Split(',')[0],".dll");
				
				//File.WriteAllText(directory+DateTime.Now.Millisecond.ToString()+".txt",args.Name+"//"+filePath);

				if(!File.Exists(filePath))
				{
					return null;
				}
				else
				{
					return Assembly.Load(File.ReadAllBytes(filePath));
				}
			};
		}
    }
}
