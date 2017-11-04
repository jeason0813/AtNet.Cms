﻿using AtNet.Cms.Service;
using AtNet.Cms.ServiceContract;
using StructureMap;

namespace AtNet.Cms.ServiceUtil
{
    internal class RedirectCallService:ICmsServiceProvider
    {
        public RedirectCallService()
        {
            ServiceInit.Initialize();
        }
        public ISiteServiceContract SiteService
        {
            get {
                return ObjectFactory.GetInstance<ISiteServiceContract>();
            }
        }

        public IArchiveServiceContract ArchiveService
        {
            get
            {
                return ObjectFactory.GetInstance<IArchiveServiceContract>();
            }
        }

        public IContentServiceContract ContentService
        {
            get {
                return ObjectFactory.GetInstance<IContentServiceContract>();
            }
        }
    }
}
