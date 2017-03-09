using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticSlots
{
    class Filesystem
    {
        DateTime session;

        /// <summary>
        /// Filesystem constructor.
        /// </summary>
        public Filesystem()
        {
            session = DateTime.Now;
        }


        // API
        // ==============================================================================

        public void WriteSlotMachineGenotype(SlotMachine slotMachine)
        {
            string[] genotype = slotMachine.chromosome.GetEncodedGenotypeString();

            //slotMachine.chromosome.PrintGenotype();

            string path = @"C:\\Users\\Public\\Genotypes\\" + session.ToFileTime();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllLines(path + "\\"+slotMachine.rtp+"_"+slotMachine.winRate+".txt", genotype);
        }
    }
}
