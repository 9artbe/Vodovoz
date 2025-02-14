﻿using System;
using NHibernate;
using NHibernate.Criterion;
using QS.DomainModel.UoW;
using QS.Project.Domain;
using QS.Services;
using QS.ViewModels;
using Vodovoz.Domain;
using Vodovoz.Domain.Goods;
using Vodovoz.EntityRepositories.RentPackages;

namespace Vodovoz.ViewModels.ViewModels.Rent
{
    public class PaidRentPackageViewModel : EntityTabViewModelBase<PaidRentPackage>
    {
	    private readonly IRentPackageRepository _rentPackageRepository;

	    public PaidRentPackageViewModel(
            IEntityUoWBuilder uowBuilder,
            IUnitOfWorkFactory unitOfWorkFactory,
            ICommonServices commonServices,
            IRentPackageRepository rentPackageRepository) : base(uowBuilder, unitOfWorkFactory, commonServices)
        {
	        _rentPackageRepository = rentPackageRepository ?? throw new ArgumentNullException(nameof(rentPackageRepository));

	        NomenclatureCriteria = UoW.Session.CreateCriteria<Nomenclature>();
	        DepositNomenclatureCriteria = NomenclatureCriteria.Add(Restrictions.Eq("Category", NomenclatureCategory.deposit));

	        ConfigureValidateContext();
        }
        
        public ICriteria DepositNomenclatureCriteria { get; }
        public ICriteria NomenclatureCriteria { get; }
        
        private void ConfigureValidateContext()
        {
	        ValidationContext.ServiceContainer.AddService(typeof(IRentPackageRepository), _rentPackageRepository);
        }
    }
}
