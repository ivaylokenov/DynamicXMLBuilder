namespace DynamicXmlBuilderTest
{
    using System;

    using DynamicXmlBuilder;

    public class Program
    {
        public static void Main()
        {
            string xml = XmlBuilder.Create()
                .Customer._b_
                    .Name("Ivaylo", new { id = "unikat", change = "clickni" })
                    .Phone("0888-888-888")
                    .Address._b_
                        .Street("In da gethou", new { @class = "css bate" })
                        .City("Kjustendja Baby")
                        .Zip("Rar")
                    ._d_
                ._d_
                .Build();

            // prints
            // <Customer>
            //     <Name id="unikat" change="clickni">Ivaylo</Name>
            //     <Phone>0888-888-888</Phone>
            //     <Address>
            //         <Street class="css bate">In da gethou</Street>
            //         <City>Kjustendja Baby</City>
            //         <Zip>Rar</Zip>
            //     </Address>
            // </Customer>

            Console.WriteLine(xml);
        }
    }
}
