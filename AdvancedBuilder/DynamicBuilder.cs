using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.IO;

namespace AdvancedBuilder
{
    class DynamicBuilder
    {
        public string Stub { get; set; }
        public string Namespace_and_class { get; set; }
        public bool Is_static { get; set; }

        public DynamicBuilder(Type type, string stub_assembly_path)
        {
            Stub = Path.GetFullPath(stub_assembly_path);
            Namespace_and_class = type.FullName;
            Is_static = type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        /// Check if OpCode is supported
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool IsSupportedOpCode(OpCode code)
        {
            return (code == OpCodes.Ldstr ||
                   code == OpCodes.Ldc_I4_1 ||
                   code == OpCodes.Ldc_I4_0);
        }

        public bool Build(Type type, string output_path)
        {
            //string constructor = Is_static ? ".cctor" : ".ctor";

            string constructor = ".cctor";

            //load stub (assembly)
            var assembly = AssemblyDef.Load(Stub);

            //get properties from settings class (provided, structure must be the same as the one we're writing to)
            var props = type.GetProperties();

            //scan for the settings class in the stub
            MethodDef method = null;
            foreach (var type_definition in assembly.ManifestModule.Types) //Modules[0]
                if (type_definition.FullName == Namespace_and_class) //xClient.Config.Settings
                {
                    var mduser = new MethodDefUser(".cctor", MethodSig.CreateInstance(assembly.ManifestModule.CorLibTypes.Void),
                            MethodImplAttributes.IL | MethodImplAttributes.Managed,
                            MethodAttributes.Public |
                            MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
                    type_definition.Methods.Add(mduser);
                    mduser.Body = new CilBody();
                    method = mduser;
                }
                    //foreach (MethodDef method_definition in type_definition.Methods)
                    //    if (method_definition.Name == constructor)
                    //    {
                    //        method = method_definition;
                    //    }


            
            

            //start replacing the initial property in settings .ctor
            if (method != null)
            {

                //int i = 0;
                //var instructions = method.Body.Instructions;
                //Console.WriteLine(instructions.Count);
                //foreach(Instruction instruction in instructions)
                //{
                //    Console.WriteLine(instruction.ToString());
                //}
                
                foreach (var p in props)
                {
                    if(p.GetValue(null, null).GetType() == typeof(string))
                    {
                        var inst = Instruction.Create(OpCodes.Ldstr, (string)p.GetValue(null, null));
                        method.Body.Instructions.Add(inst);
                    }
                   
                }

                /* Old code
                int i = 0;
                var instructions = method.Body.Instructions;
                for (int j = 0; j < instructions.Count; j++)
                {
                    if (IsSupportedOpCode(instructions[j].OpCode))
                    {
                        var value = props[j - i].GetValue(SettingsClass, null);
                        var inst_value = value.ToInstruction();
                        if (inst_value != null)
                            instructions[j] = inst_value;
                    }
                    else
                    {
                        i++;
                    }
                }*/
            }else
            {
                throw new InvalidStubStructureException("Incompatible stub!");
            }

            //write output
            assembly.Write(output_path);

            return false;
        }
    }
}

public class InvalidStubStructureException : Exception
{
    public InvalidStubStructureException(string message) : base(message)
    {
    }
}

public static class PropertyInfoExtension
{
    /// <summary>
    /// Create a call to set the value of a property, to be placed in the .ctor of a Settings class
    /// </summary>
    /// <param name="pi"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Instruction CreateCall(this System.Reflection.PropertyInfo pi, object value)
    {
        var arg = Instruction.Create(OpCodes.Ldarg_0); // 1 argument, index 0
        var instruction = value.ToInstruction(); //ldstr or whatever else it is (this is dynamic thanks to below method)
        if (instruction == null) return null;
        var call = Instruction.Create(OpCodes.Call, 0x000000); //call Set_property;
        var nop = Instruction.Create(OpCodes.Nop); //nop

        //not sure how to place these instructions in a body correctly..
        //also we might have to erase the old set instructions in the old .ctor
        return null;
    }

    public static OpCode ToOpCode(this System.Reflection.PropertyInfo property)
    {
        if(property.GetValue(null,null).GetType() == typeof(string))
        {
            return OpCodes.Ldstr;
        }

        return OpCodes.Nop;
    }

    /// <summary>
    /// Converts a value to IL instruction based on type
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Instruction ToInstruction(this object value)
    {
        if (value.GetType() == typeof(bool))
        {
            if ((bool)value)
                return Instruction.Create(OpCodes.Ldc_I4_1);
            else
                return Instruction.Create(OpCodes.Ldc_I4_0);
        }
        else if (value.GetType() == typeof(string))
        {
            return Instruction.Create(OpCodes.Ldstr, (string)value);
        }
        else
        {
            return null;
        }
    }
}