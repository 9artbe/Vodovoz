using QS.DomainModel.UoW;
using QS.Project.Journal;
using QS.Project.Journal.EntitySelector;
using QS.Services;
using Vodovoz.TempAdapters;
using Vodovoz.ViewModels.Journals.FilterViewModels;
using Vodovoz.ViewModels.Journals.JournalFactories;
using Vodovoz.ViewModels.Journals.JournalViewModels.Cash;
using VodovozInfrastructure.Interfaces;

namespace Vodovoz.ViewModels.Journals.JournalSelectors
{
    public class IncomeCategoryAutoCompleteSelectorFactory:
        IncomeCategorySelectorFactory, IEntityAutocompleteSelectorFactory
    {
	    public IncomeCategoryAutoCompleteSelectorFactory(
		    ICommonServices commonServices,
		    IncomeCategoryJournalFilterViewModel filterViewModel,
		    IFileChooserProvider fileChooserProvider,
		    IEmployeeJournalFactory employeeJournalFactory,
		    ISubdivisionJournalFactory subdivisionJournalFactory)
		    : base(commonServices, filterViewModel, fileChooserProvider, employeeJournalFactory, subdivisionJournalFactory)
	    {
		    
	    }

        public IEntityAutocompleteSelector CreateAutocompleteSelector(bool multipleSelect = false)
        {
            IncomeCategoryJournalViewModel selectorViewModel = new IncomeCategoryJournalViewModel(
                filter,
                UnitOfWorkFactory.GetDefaultFactory,
                commonServices,
                fileChooserProvider,
                _employeeJournalFactory,
                _subdivisionJournalFactory)
            {
                SelectionMode = JournalSelectionMode.Single
            };
			
            return selectorViewModel;
        }
    }
}