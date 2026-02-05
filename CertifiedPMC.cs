using SPTarkov.DI.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CertifiedPMC
{
    [Injectable]
    public class CertifiedPMC
    {
        private readonly SaveServer _saveServer;
        private readonly ISptLogger<CertifiedPMC> _logger;

        private int minValue = 0;
        private int maxValue = 5100;

        public CertifiedPMC(SaveServer saveServer, ISptLogger<CertifiedPMC> logger) 
        {
            _saveServer = saveServer;
            _logger = logger;
        }


        public void ModifySkills(MongoId sessionId)
        {
            var profile = _saveServer.GetProfile(sessionId);
        }
    }
}
