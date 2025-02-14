using System;
using System.IO;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using QS.DomainModel.UoW;
using QS.Project.Domain;
using QS.Project.Journal;
using QS.Services;
using Vodovoz.Domain.Cash;
using Vodovoz.TempAdapters;
using Vodovoz.ViewModels.Journals.FilterViewModels;
using Vodovoz.ViewModels.Journals.FilterViewModels.Enums;
using Vodovoz.ViewModels.Journals.JournalFactories;
using Vodovoz.ViewModels.Journals.JournalNodes;
using Vodovoz.ViewModels.ViewModels.Cash;
using VodovozInfrastructure.Interfaces;

namespace Vodovoz.ViewModels.Journals.JournalViewModels.Cash
{
    public class IncomeCategoryJournalViewModel : FilterableSingleEntityJournalViewModelBase
        <
            IncomeCategory,
            IncomeCategoryViewModel,
            IncomeCategoryJournalNode,
            IncomeCategoryJournalFilterViewModel
        >
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IFileChooserProvider fileChooserProvider;
        private readonly IEmployeeJournalFactory _employeeJournalFactory;
        private readonly ISubdivisionJournalFactory _subdivisionJournalFactory;

        public IncomeCategoryJournalViewModel(
            IncomeCategoryJournalFilterViewModel journalFilterViewModel,
            IUnitOfWorkFactory unitOfWorkFactory,
            ICommonServices commonServices,
            IFileChooserProvider fileChooserProvider,
            IEmployeeJournalFactory employeeJournalFactory,
            ISubdivisionJournalFactory subdivisionJournalFactory
        ) : base(journalFilterViewModel, unitOfWorkFactory, commonServices)
        {
            this.unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
            this.fileChooserProvider = fileChooserProvider ?? throw new ArgumentNullException(nameof(fileChooserProvider));
            _employeeJournalFactory = employeeJournalFactory ?? throw new ArgumentNullException(nameof(employeeJournalFactory));
            _subdivisionJournalFactory = subdivisionJournalFactory ?? throw new ArgumentNullException(nameof(subdivisionJournalFactory));

            TabName = "Категории прихода";
            
            UpdateOnChanges(
                typeof(IncomeCategory),
                typeof(Subdivision)
            );
        }

        protected override Func<IUnitOfWork, IQueryOver<IncomeCategory>> ItemsSourceQueryFunction => (uow) => {
            IncomeCategoryJournalNode resultAlias = null;

            var query = uow.Session.QueryOver<IncomeCategory>();

            IncomeCategory level1Alias = null;
            IncomeCategory level2Alias = null;
            IncomeCategory level3Alias = null;
            IncomeCategory level4Alias = null;
            IncomeCategory level5Alias = null;
            Subdivision subdivisionAlias = null;
            // При цепочке связи:
            // 5 <- 4 <- 3 <- 2 <- 1
            // Уровни распределяются как
            // lvl1 - 5
            // lvl2 - 4|5
            // lvl3 - 3|4|5
            // lvl4 - 2|3|4|5
            // lvl5 - 1|2|3|4|5
            query = uow.Session.QueryOver<IncomeCategory>(() => level1Alias)
                .Left.JoinAlias(() => level1Alias.Parent, () => level2Alias)
                .Left.JoinAlias(() => level2Alias.Parent, () => level3Alias)
                .Left.JoinAlias(() => level3Alias.Parent, () => level4Alias)
                .Left.JoinAlias(() => level4Alias.Parent, () => level5Alias)
                .Left.JoinAlias(() => level1Alias.Subdivision, () => subdivisionAlias);
            
            if (!FilterViewModel.ShowArchive) 
                query.Where(x => !x.IsArchive);
            switch (FilterViewModel.Level)
            {
                case LevelsFilter.Level1:
                    query.Where(Restrictions.IsNull(Projections.Property(() => level2Alias.Id)));
                    break;
                case LevelsFilter.Level2:
                    query.Where(Restrictions.IsNull(Projections.Property(() => level3Alias.Id)));
                    break;
                case LevelsFilter.Level3:
                    query.Where(Restrictions.IsNull(Projections.Property(() => level4Alias.Id)));
                    break;
                case LevelsFilter.Level4:
                    query.Where(Restrictions.IsNull(Projections.Property(() => level5Alias.Id)));
                    break;
            }
            
            query.SelectList(list => list
                    .Select(x => x.Id).WithAlias(() => resultAlias.Id)
                    .Select(() => level1Alias.Name).WithAlias(() => resultAlias.Level5)
                    .Select(() => level2Alias.Name).WithAlias(() => resultAlias.Level4)
                    .Select(() => level3Alias.Name).WithAlias(() => resultAlias.Level3)
                    .Select(() => level4Alias.Name).WithAlias(() => resultAlias.Level2)
                    .Select(() => level5Alias.Name).WithAlias(() => resultAlias.Level1)
                    .Select(() => FilterViewModel.Level).WithAlias(() => resultAlias.LevelFilter)
                    .Select(() => subdivisionAlias.ShortName).WithAlias(() => resultAlias.Subdivision)
                    .Select(x => x.IsArchive).WithAlias(() => resultAlias.IsArchive)
                ).TransformUsing(Transformers.AliasToBean<IncomeCategoryJournalNode>())
                .OrderBy(x => x.Name);
            
            query.Where(
                GetSearchCriterion(
                    () => level5Alias.Name,
                    () => level4Alias.Name,
                    () => level3Alias.Name,
                    () => level2Alias.Name,
                    () => level1Alias.Name,
                    () => level5Alias.Id,
                    () => level4Alias.Id,
                    () => level3Alias.Id,
                    () => level2Alias.Id,
                    () => level1Alias.Id
                )
            );
            return query;
        };
        
        protected override Func<IncomeCategoryViewModel> CreateDialogFunction => () => new IncomeCategoryViewModel(
            EntityUoWBuilder.ForCreate(),
            unitOfWorkFactory,
            commonServices,
            fileChooserProvider,
            FilterViewModel,
            _employeeJournalFactory,
            _subdivisionJournalFactory
        );
        protected override Func<IncomeCategoryJournalNode, IncomeCategoryViewModel> OpenDialogFunction =>
            node => new IncomeCategoryViewModel(
                EntityUoWBuilder.ForOpen(node.Id), 
                unitOfWorkFactory, 
                commonServices, 
                fileChooserProvider, 
                FilterViewModel,
                _employeeJournalFactory,
                _subdivisionJournalFactory
        );

        protected override void CreatePopupActions()
        {
            base.CreatePopupActions();
            NodeActionsList.Add(new JournalAction(
                "Экспорт", 
                x => true, 
                x => true, 
                selectedItems => {
                    var selectedNodes = selectedItems.Cast<IncomeCategoryJournalNode>();
                    StringBuilder CSVbuilder = new StringBuilder();
                    foreach (IncomeCategoryJournalNode incomeCategoryJournalNode in Items)
                    {
                        CSVbuilder.Append(incomeCategoryJournalNode.Level1 + ", ");
                        CSVbuilder.Append(incomeCategoryJournalNode.Level2 + ", ");
                        CSVbuilder.Append(incomeCategoryJournalNode.Level3 + ", ");
                        CSVbuilder.Append(incomeCategoryJournalNode.Level4 + ", ");
                        CSVbuilder.Append(incomeCategoryJournalNode.Level5 + ", ");
                        CSVbuilder.Append(incomeCategoryJournalNode.Subdivision + "\n");
                    }

                    var fileChooserPath = fileChooserProvider.GetExportFilePath();
                    var res = CSVbuilder.ToString();
                    if (fileChooserPath == "") return;
                    Stream fileStream = new FileStream(fileChooserPath, FileMode.Create);
                    using (StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.GetEncoding("Windows-1251")))  
                    {  
                        writer.Write("\"sep=,\"\n");
                        writer.Write(res.ToString());
                    }               
                    fileChooserProvider.CloseWindow(); 
                })
            );

            PopupActionsList.Add(new JournalAction(
                "Архивировать",
                x => true, 
                x => true,
                selectedItems => {
                    var selectedNodes = selectedItems.Cast<IncomeCategoryJournalNode>();
                    var selectedNode = selectedNodes.FirstOrDefault();
                    if(selectedNode != null)
                    {
                        selectedNode.IsArchive = true;
                        using (var uow = UnitOfWorkFactory.CreateForRoot<IncomeCategory>(selectedNode.Id))
                        {
                            uow.Root.SetIsArchiveRecursively(true);
                            uow.Save();                   
                            uow.Commit();
                        } 
                    }
                })
            );
        }
    }
}