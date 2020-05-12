﻿using System;

namespace Wells.Tools
    /// <summary>
    {
        /// <summary>
        {
            //设置泛型类型的默认值 
            T result = default(T);
            //获得类型 
            Type t = getType(url, getWsClassName(url));
            try
            {
                //依据类型创建实例 
                object obj = createInstance(t);
                //调用方法 
                result = InvokeMethod<T>(obj, t, methodName, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        
        /// <summary>
        {
            T result = default(T);
            //依据方法名获取方法信息 
            System.Reflection.MethodInfo mi = t.GetMethod(methodName);
            //调用实例方法 
            result = (T)mi.Invoke(InstanceObject, args);
            return result;
        }
        
        /// <summary>
        {
            Type result = null;
            string @namespace = "Wells.Tools.Temp.WebService";
            if (string.IsNullOrEmpty(className))
            {
                className = clsWebService.getWsClassName(url);
            }
            //获取WSDL 
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url + "?WSDL");
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            //生成客户端代理类代码 
            CodeNamespace np = new CodeNamespace(@namespace);
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(np);
            sdi.Import(np, ccu);
            //获取c#编译器的实例 
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
            //设定编译参数 
            CompilerParameters param = new CompilerParameters();
            param.GenerateExecutable = false;
            param.GenerateInMemory = true;
            param.ReferencedAssemblies.Add("System.dll");
            param.ReferencedAssemblies.Add("System.XML.dll");
            param.ReferencedAssemblies.Add("System.Web.Services.dll");
            param.ReferencedAssemblies.Add("System.Data.dll");
            //编译代理类 
            CompilerResults cr = provider.CompileAssemblyFromDom(param, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            //生成代理实例，并调用方法 
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            //反射获取类型 
            result = assembly.GetType(@namespace + "." + className, true, true);
            return result;
        }

        /// <summary>
        {
            //获取类型的默认值 
            object result = null;
            try
            {
                //创建实例类型 
                result = Activator.CreateInstance(t);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        
        /// <summary>
        {
            //依据类型获得类型属性 
            System.Reflection.PropertyInfo pi = t.GetProperty(propertyName, BindingFlags.Public);
            //给实例对象属性赋值 
            pi.SetValue(InstanceObject, valueObj, null);
        }
        
        /// <summary>
        {
            string result = string.Empty;
            string[] parts = url.Split('/');
            string fileName = parts[parts.Length - 1];
            result = fileName.Split('.')[0];
            return result;
        }
    }