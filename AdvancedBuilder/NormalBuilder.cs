using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Linq;

namespace AdvancedBuilder
{
    public class NormalBuilder
    {
        public string Stub { get; set; }
        public string FullName { get; set; }

        public void Build(string output_assembly)
        {
            var assembly = AssemblyDef.Load(Stub);

            var module = assembly.ManifestModule;
            if(module != null)
            {
                var settings = module.GetTypes().Where(type => type.FullName == FullName).FirstOrDefault();
                if (settings != null)
                {
                    var constructor = settings.FindMethod(".cctor");
                    if(constructor != null)
                    {
                        //install_path (string)
                        constructor.Body.Instructions[0].Operand = TestStub.Settings.Install_path;

                        //install (bool)
                        constructor.Body.Instructions[2].OpCode = FromBool(TestStub.Settings.Install);

                        //secret (int)
                        constructor.Body.Instructions[4].Operand = TestStub.Settings.Secret;
                        constructor.Body.Instructions[4].OpCode = OpCodes.Ldc_I4; //required or it'll turn into ldc_i4_0

                        //something_secret (string)
                        constructor.Body.Instructions[6].Operand = TestStub.Settings.Something_secret;


                        //...increase by 2
                    }
                }
            }

            assembly.Write(output_assembly);
        }

        private OpCode FromBool(bool a)
        {
            return a ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0;
        }
    }
}
