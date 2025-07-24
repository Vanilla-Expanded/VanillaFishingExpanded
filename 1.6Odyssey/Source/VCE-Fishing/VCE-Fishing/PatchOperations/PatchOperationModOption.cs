using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VCE_Fishing.Options;
using Verse;

namespace VCE_Fishing
{
    public class PatchOperationModOption : PatchOperation
    {

        private PatchOperation match;

        private PatchOperation nomatch;

        protected override bool ApplyWorker(XmlDocument xml)
        {

            if (VCE_Fishing_Mod.settings.VCEF_UseWorktype)
            {
                if (match != null)
                {
                    return match.Apply(xml);
                }
            }
            else if (nomatch != null)
            {
                return nomatch.Apply(xml);
            }
            return true;
        }


    }
}