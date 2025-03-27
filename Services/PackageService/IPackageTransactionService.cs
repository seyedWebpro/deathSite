using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;
using deathSite.Model;

namespace deathSite.Services.PackageService
{
   public interface IPackageTransactionService
{
    Task<DeceasedPackage> HandleNewPackageRegistration(Factors factor, Package package);
    Task HandlePackageRenewal(Factors factor, Package package);
    Task HandlePackageUpgrade(Factors factor, Package newPackage);
}


}