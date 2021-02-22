using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaintenanceModeManager.Interfaces;
using MaintenanceModeManager.Schemas;
using NPoco;
using Umbraco.Core.Scoping;
using Umbraco.Web;

namespace MaintenanceModeManager
{
    public class Config : IMaintenanceModeManagerConfig
    {
        private const string CacheKey = "maintenanceModeManagerConfig";
        private readonly IScopeProvider scopeProvider;
        private readonly IUmbracoContextFactory umbracoContextFactory;

        public Config(IScopeProvider scopeProvider, IUmbracoContextFactory umbracoContextFactory)
        {
            this.scopeProvider = scopeProvider ?? throw new ArgumentNullException(nameof(scopeProvider));
            this.umbracoContextFactory = umbracoContextFactory ?? throw new ArgumentNullException(nameof(umbracoContextFactory));
        }

        public int GetMaintenancePage(int parentId)
        {
            using (var umbracoContext = umbracoContextFactory.EnsureUmbracoContext())
            {
                var parentNode = umbracoContext.UmbracoContext.Content.GetById(parentId);
                return parentNode != null ? GetMaintenancePage(parentNode.Key) : 0;
            }
        }

        public int GetMaintenancePage(Guid parentKey)
        {
            using (var umbracoContext = umbracoContextFactory.EnsureUmbracoContext())
            {
                var x = ConfiguredPages.FirstOrDefault(p => p.ParentId == parentKey);
                var page = x != null ? umbracoContext.UmbracoContext.Content.GetById(x.MaintenanceModePageId) : null;
                return page != null ? page.Id : 0;
            }
        }

        public void SetMaintenancePage(int parentId, int pageNotFoundId, bool refreshCache)
        {
            using (var umbracoContext = umbracoContextFactory.EnsureUmbracoContext())
            {
                var parentPage = umbracoContext.UmbracoContext.Content.GetById(parentId);
                var pageNotFoundPage = umbracoContext.UmbracoContext.Content.GetById(pageNotFoundId);
                SetMaintenancePage(parentPage.Key, pageNotFoundPage != null ? pageNotFoundPage.Key : Guid.Empty, refreshCache);
            }
        }

        public void SetMaintenancePage(Guid parentKey, Guid pageNotFoundKey, bool refreshCache)
        {

            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                var page = db.Query<MaintenanceModeManagerSchema>().Where(p => p.ParentId == parentKey).FirstOrDefault();
                if (page == null && !Guid.Empty.Equals(pageNotFoundKey))
                {
                    // create the page
                    db.Insert<MaintenanceModeManagerSchema>(new MaintenanceModeManagerSchema { ParentId = parentKey, MaintenanceModePageId = pageNotFoundKey });
                }
                else if (page != null)
                {
                    if (Guid.Empty.Equals(pageNotFoundKey))
                        db.Delete(page);
                    else
                    {
                        // update the existing page
                        page.MaintenanceModePageId = pageNotFoundKey;
                        db.Update(MaintenanceModeManagerSchema.TableName, "ParentId", page);
                    }
                }
                scope.Complete();
            }

            if (refreshCache)
                RefreshCache();
        }

        public void RefreshCache()
        {
            HttpRuntime.Cache.Remove(CacheKey);
            LoadFromDb();
        }

        private IEnumerable<MaintenanceModeManagerSchema> ConfiguredPages
        {
            get
            {
                var us = (IEnumerable<MaintenanceModeManagerSchema>)HttpRuntime.Cache[CacheKey] ?? LoadFromDb();
                return us;
            }
        }

        private IEnumerable<MaintenanceModeManagerSchema> LoadFromDb()
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                var sql = new Sql().Select("*").From(MaintenanceModeManagerSchema.TableName);
                var pages = db.Fetch<MaintenanceModeManagerSchema>(sql);
                HttpRuntime.Cache.Insert(CacheKey, pages);
                scope.Complete();
                return pages;
            }
        }
    }
}