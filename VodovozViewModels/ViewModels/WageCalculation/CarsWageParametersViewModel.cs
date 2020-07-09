﻿using System;
using QS.Services;
using QS.ViewModels;
using System.Collections.Generic;
using System.Linq;
using QS.Commands;
using QS.DomainModel.UoW;
using Vodovoz.Domain.WageCalculation;
using Vodovoz.EntityRepositories.WageCalculation;
using System.Data.Bindings.Collections.Generic;
using QS.DomainModel.Entity;
using QS.Navigation;

namespace Vodovoz.ViewModels.WageCalculation
{
	// public class CarsWageParametersViewModel : DialogTabViewModelBase
	// {
	// 	private readonly IWageCalculationRepository wageCalculationRepository;
	// 	private readonly ICommonServices commonServices;
	//
	// 	public event EventHandler OnParameterNodesUpdated;
	//
	// 	public CarsWageParametersViewModel(IUnitOfWorkFactory unitOfWorkFactory, IWageCalculationRepository wageCalculationRepository, ICommonServices commonServices, INavigationManager navigationManager) 
	// 		: base(unitOfWorkFactory, commonServices.InteractiveService, navigationManager)
	// 	{
	// 		TabName = "Ставки для автомобилей компании";
	//
	// 		this.wageCalculationRepository = wageCalculationRepository ?? throw new ArgumentNullException(nameof(wageCalculationRepository));
	// 		this.commonServices = commonServices ?? throw new ArgumentNullException(nameof(commonServices));
	//
	// 		ObservableWageParameters.ElementAdded += (aList, aIdx) => WageParametersUpdated();
	// 		ObservableWageParameters.ElementRemoved += (aList, aIdx, aObject) => WageParametersUpdated();
	// 		CreateCommands();
	// 		LoadData();
	// 	}
	//
	// 	private DateTime? startDate;
	// 	[PropertyChangedAlso(nameof(CanChangeWageCalculation))]
	// 	public virtual DateTime? StartDate {
	// 		get => startDate;
	// 		set => SetField(ref startDate, value, () => StartDate);
	// 	}
	//
	// 	IList<WageParameter> wageParameters = new List<WageParameter>();
	// 	public virtual IList<WageParameter> WageParameters {
	// 		get => wageParameters;
	// 		set => SetField(ref wageParameters, value, () => WageParameters);
	// 	}
	//
	// 	GenericObservableList<WageParameter> observableWageParameters;
	// 	//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
	// 	public virtual GenericObservableList<WageParameter> ObservableWageParameters {
	// 		get {
	// 			if(observableWageParameters == null)
	// 				observableWageParameters = new GenericObservableList<WageParameter>(WageParameters);
	// 			return observableWageParameters;
	// 		}
	// 	}
	//
	// 	public virtual IList<EmployeeWageParameterNode> WageParameterNodes => ObservableWageParameters.Select(x => new EmployeeWageParameterNode(x)).ToList();
	//
	// 	private void WageParametersUpdated()
	// 	{
	// 		OnParameterNodesUpdated?.Invoke(this, EventArgs.Empty);
	// 	}
	//
	// 	private void LoadData()
	// 	{
	// 		var items = wageCalculationRepository.GetWageParameters(UoW, WageParameterTargets.ForOurCars);
	// 		ObservableWageParameters.Clear();
	// 		foreach(var item in items) {
	// 			ObservableWageParameters.Add(item);
	// 		}
	// 	}
	//
	// 	public virtual bool CheckStartDateForNewWageParameter(DateTime newStartDate)
	// 	{
	// 		WageParameter oldWageParameter = ObservableWageParameters.FirstOrDefault(x => x.EndDate == null);
	// 		if(oldWageParameter == null) {
	// 			return true;
	// 		}
	//
	// 		return oldWageParameter.StartDate < newStartDate;
	// 	}
	//
	// 	public virtual void ChangeWageParameter(WageParameter wageParameter)
	// 	{
	// 		if(wageParameter == null) {
	// 			throw new ArgumentNullException(nameof(wageParameter));
	// 		}
	//
	// 		if(StartDate == null) {
	// 			ShowErrorMessage("Необходимо выбрать время");
	// 			return;
	// 		}
	//
	// 		wageParameter.StartDate = StartDate.Value.AddTicks(1);
	// 		WageParameter oldWageParameter = ObservableWageParameters.FirstOrDefault(x => x.EndDate == null);
	// 		if(oldWageParameter != null) {
	// 			if(oldWageParameter.StartDate > startDate) {
	// 				throw new InvalidOperationException("Нельзя создать новую запись с датой более ранней уже существующей записи. Неверно выбрана дата");
	// 			}
	// 			oldWageParameter.EndDate = StartDate;
	// 		}
	// 		ObservableWageParameters.Add(wageParameter);
	// 	}
	//
	// 	private void CreateCommands()
	// 	{
	// 		CreateChangeWageParameterCommand();
	// 		CreateOpenWageParameterCommand();
	// 		CreateChangeWageStartDateCommand();
	// 	}
	//
	// 	#region ChangeWageParameterCommand
	//
	// 	public DelegateCommand ChangeWageParameterCommand { get; private set; }
	//
	// 	public virtual bool CanChangeWageCalculation => StartDate.HasValue && CheckStartDateForNewWageParameter(StartDate.Value);
	//
	// 	private void CreateChangeWageParameterCommand()
	// 	{
	// 		ChangeWageParameterCommand = new DelegateCommand(
	// 			() => {
	// 				EmployeeWageParameterViewModel newEmployeeWageParameterViewModel = new EmployeeWageParameterViewModel(UoW, WageParameterTargets.ForOurCars, commonServices, NavigationManager);
	// 				newEmployeeWageParameterViewModel.OnWageParameterCreated += (sender, wageParameter) => {
	// 					ChangeWageParameter(wageParameter);
	// 				};
	// 				TabParent.AddSlaveTab(this, newEmployeeWageParameterViewModel);
	// 			},
	// 			() => CanChangeWageCalculation
	// 		);
	//
	// 		ChangeWageParameterCommand.CanExecuteChangedWith(this, x => x.CanChangeWageCalculation);
	// 	}
	//
	// 	#endregion ChangeWageParameterCommand
	//
	// 	#region ChangeWageStartDateCommand
	//
	// 	public DelegateCommand<EmployeeWageParameterNode> ChangeWageStartDateCommand { get; private set; }
	//
	// 	private void CreateChangeWageStartDateCommand()
	// 	{
	// 		ChangeWageStartDateCommand = new DelegateCommand<EmployeeWageParameterNode>(
	// 			(node) => {
	// 				if(!AskQuestion(
	// 					"Внимание! Будет произведено изменение даты в уже имеющемся расчете зарплаты, " +
	// 					"документы попадающие в этот интервал будут пересчитываться по другому расчету! " +
	// 					"Продолжить?", "Внимание!")) {
	// 					return;
	// 				}
	//
	// 				var previousParameter = GetPreviousParameter(node.EmployeeWageParameter.StartDate);
	// 				if(previousParameter != null) {
	// 					previousParameter.EndDate = StartDate.Value.AddTicks(-1);
	// 				}
	// 				node.EmployeeWageParameter.StartDate = StartDate.Value;
	// 				WageParametersUpdated();
	//
	// 			},
	// 			(node) => {
	// 				if(node == null || !StartDate.HasValue) {
	// 					return false;
	// 				}
	// 				var previousParameterByDate = GetPreviousParameter(StartDate.Value);
	// 				var previousParameterBySelectedParameter = GetPreviousParameter(node.EmployeeWageParameter.StartDate);
	//
	// 				bool noConflictWithEndDate = !node.EmployeeWageParameter.EndDate.HasValue || node.EmployeeWageParameter.EndDate.Value > StartDate;
	// 				bool noConflictWithPreviousStartDate = (previousParameterByDate == null && previousParameterBySelectedParameter == null) || (previousParameterBySelectedParameter != null && previousParameterBySelectedParameter.StartDate < StartDate);
	//
	// 				return StartDate.HasValue && noConflictWithEndDate && noConflictWithPreviousStartDate;
	// 			}
	// 		);
	// 		ChangeWageStartDateCommand.CanExecuteChangedWith(this, x => x.StartDate);
	// 	}
	//
	// 	private WageParameter GetPreviousParameter(DateTime date)
	// 	{
	// 		return ObservableWageParameters
	// 					.Where(x => x.EndDate != null)
	// 					.Where(x => x.EndDate <= date)
	// 					.OrderByDescending(x => x.EndDate)
	// 					.FirstOrDefault();
	// 	}
	//
	// 	#endregion ChangeWageStartDateCommand
	//
	// 	#region OpenWageParameterCommand
	//
	// 	public DelegateCommand<EmployeeWageParameterNode> OpenWageParameterCommand { get; private set; }	
	//
	// 	private void CreateOpenWageParameterCommand()
	// 	{
	// 		OpenWageParameterCommand = new DelegateCommand<EmployeeWageParameterNode>(
	// 			(node) => {
	// 				EmployeeWageParameterViewModel employeeWageParameterViewModel = new EmployeeWageParameterViewModel(node.EmployeeWageParameter, UoW, commonServices, NavigationManager);
	// 				TabParent.AddTab(employeeWageParameterViewModel, this);
	// 			},
	// 			(node) => node != null
	// 		);
	// 	}
	//
	// 	#endregion OpenWageParameterCommand
	//
	// 	public override bool Save(bool close)
	// 	{
	// 		foreach(var wagePararmeter in WageParameters) {
	// 			UoW.Save(wagePararmeter);
	// 		}
	// 		UoW.Commit();
	// 		Close(false, CloseSource.Save);
	// 		return true;
	// 	}
	// }
}
