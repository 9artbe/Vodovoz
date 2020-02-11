﻿using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Gamma.GtkWidgets;
using Gamma.Widgets;
using Gtk;
using NLog;
using QS.DomainModel.UoW;
using QSWidgetLib;
using Vodovoz.Domain.Contacts;
using Vodovoz.EntityRepositories;
using Vodovoz.Parameters;

namespace Vodovoz.Views.Contacts
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PhonesView : Gtk.Bin
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private GenericObservableList<Phone> phonesList;
		private IList<PhoneType> phoneTypes;
		private IUnitOfWork uow;
		private IPhoneRepository phoneRepository;

		private bool isReadOnly;
		public bool IsReadOnly {
			get { return isReadOnly; }
			set {
				if(isReadOnly == value)
					return;
				isReadOnly = value;
				SetEditable();
			}
		}

		public IUnitOfWork UoW {
			get { return uow; }
			set {
				uow = value;
				phoneTypes = phoneRepository.GetPhoneTypes(uow);
			}
		}

		private IList<Phone> phones;

		public IList<Phone> Phones {
			get {
				return phones;
			}
			set {
				if(phones == value)
					return;
				phones = value;

				PhonesList = phones != null ? new GenericObservableList<Phone>(phones) : null;
			}
		}

		public GenericObservableList<Phone> PhonesList {
			get {
				return phonesList;
			}

			set {
				if(PhonesList != null)
					CleanList();

				phonesList = value;

				buttonAdd.Sensitive = phonesList != null;
				if(value != null) {
					PhonesList.ElementAdded += OnPhoneListElementAdded;
					PhonesList.ElementRemoved += OnPhoneListElementRemoved;
					if(PhonesList.Count == 0)
						PhonesList.Add(new Phone().Init(ContactParametersProvider.Instance));
					else {
						foreach(Phone phone in PhonesList)
							AddPhoneRow(phone);
					}
				}
				SetEditable();
			}
		}

		void OnPhoneListElementRemoved(object aList, int[] aIdx, object aObject)
		{
			Widget foundWidget = null;
			foreach(Widget wid in datatablePhones.AllChildren) {
				if(wid is yValidatedEntry && (wid as yValidatedEntry).Tag == aObject) {
					foundWidget = wid;
					break;
				}
			}
			if(foundWidget == null) {
				logger.Warn("Не найден виджет ассоциированный с удаленным телефоном.");
				return;
			}

			Table.TableChild child = ((Table.TableChild)(this.datatablePhones[foundWidget]));
			RemoveRow(child.TopAttach);
		}

		void OnPhoneListElementAdded(object aList, int[] aIdx)
		{
			foreach(int i in aIdx) {
				AddPhoneRow(PhonesList[i]);
			}
		}

		uint RowNum;

		public PhonesView()
		{
			this.Build();
			phoneRepository = new PhoneRepository();
			datatablePhones.NRows = RowNum = 0;
		}

		protected void OnButtonAddClicked(object sender, EventArgs e)
		{
			PhonesList.Add(new Phone().Init(ContactParametersProvider.Instance));
		}

		private void AddPhoneRow(Phone newPhone)
		{
			datatablePhones.NRows = RowNum + 1;

			var phoneDataCombo = new yListComboBox();
			phoneDataCombo.WidthRequest = 100;
			phoneDataCombo.SetRenderTextFunc((PhoneType x) => x.Name);
			phoneDataCombo.ItemsList = phoneTypes;
			phoneDataCombo.Binding.AddBinding(newPhone, e => e.PhoneType, w => w.SelectedItem).InitializeFromSource();
			datatablePhones.Attach(phoneDataCombo, (uint)0, (uint)1, RowNum, RowNum + 1, AttachOptions.Fill | AttachOptions.Expand, (AttachOptions)0, (uint)0, (uint)0);

			Gtk.Label textPhoneLabel = new Gtk.Label("+7");
			datatablePhones.Attach(textPhoneLabel, (uint)1, (uint)2, RowNum, RowNum + 1, (AttachOptions)0, (AttachOptions)0, (uint)0, (uint)0);

			var phoneDataEntry = new yValidatedEntry();
			phoneDataEntry.ValidationMode = ValidationType.phone;
			phoneDataEntry.Tag = newPhone;
			phoneDataEntry.WidthChars = 19;
			phoneDataEntry.Binding.AddBinding(newPhone, e => e.Number, w => w.Text).InitializeFromSource();
			datatablePhones.Attach(phoneDataEntry, (uint)2, (uint)3, RowNum, RowNum + 1, AttachOptions.Expand | AttachOptions.Fill, (AttachOptions)0, (uint)0, (uint)0);

			Gtk.Label textAdditionalLabel = new Gtk.Label("доб.");
			datatablePhones.Attach(textAdditionalLabel, (uint)3, (uint)4, RowNum, RowNum + 1, (AttachOptions)0, (AttachOptions)0, (uint)0, (uint)0);

			var additionalDataEntry = new yEntry();
			additionalDataEntry.WidthRequest = 50;
			additionalDataEntry.MaxLength = 10;
			additionalDataEntry.Binding.AddBinding(newPhone, e => e.Additional, w => w.Text).InitializeFromSource();
			datatablePhones.Attach(additionalDataEntry, (uint)4, (uint)5, RowNum, RowNum + 1, AttachOptions.Fill, (AttachOptions)0, (uint)0, (uint)0);

			Gtk.Label labelName = new Gtk.Label("имя:");
			datatablePhones.Attach(labelName, (uint)5, (uint)6, RowNum, RowNum + 1, (AttachOptions)0, (AttachOptions)0, (uint)0, (uint)0);

			var entryName = new yEntry();
			//entryName.WidthRequest = 50;
			entryName.MaxLength = 150;
			entryName.Binding.AddBinding(newPhone, e => e.Name, w => w.Text).InitializeFromSource();
			datatablePhones.Attach(entryName, (uint)6, (uint)7, RowNum, RowNum + 1, AttachOptions.Expand | AttachOptions.Fill, (AttachOptions)0, (uint)0, (uint)0);

			Gtk.Button deleteButton = new Gtk.Button();
			Gtk.Image image = new Gtk.Image();
			image.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-delete", global::Gtk.IconSize.Menu);
			deleteButton.Image = image;
			deleteButton.Clicked += OnButtonDeleteClicked;
			datatablePhones.Attach(deleteButton, (uint)7, (uint)8, RowNum, RowNum + 1, (AttachOptions)0, (AttachOptions)0, (uint)0, (uint)0);

			datatablePhones.ShowAll();

			RowNum++;
		}

		protected void OnButtonDeleteClicked(object sender, EventArgs e)
		{
			Table.TableChild delButtonInfo = ((Table.TableChild)(this.datatablePhones[(Widget)sender]));
			Widget foundWidget = null;
			foreach(Widget wid in datatablePhones.AllChildren) {
				if(wid is yValidatedEntry && delButtonInfo.TopAttach == (datatablePhones[wid] as Table.TableChild).TopAttach) {
					foundWidget = wid;
					break;
				}
			}
			if(foundWidget == null) {
				logger.Warn("Не найден виджет ассоциированный с удаленным телефоном.");
				return;
			}

			PhonesList.Remove((Phone)(foundWidget as yValidatedEntry).Tag);
		}

		private void RemoveRow(uint Row)
		{
			foreach(Widget w in datatablePhones.Children)
				if(((Table.TableChild)(this.datatablePhones[w])).TopAttach == Row) {
					datatablePhones.Remove(w);
					w.Destroy();
				}
			for(uint i = Row + 1; i < datatablePhones.NRows; i++)
				MoveRowUp(i);
			datatablePhones.NRows = --RowNum;
		}

		protected void MoveRowUp(uint Row)
		{
			foreach(Widget w in datatablePhones.Children)
				if(((Table.TableChild)(this.datatablePhones[w])).TopAttach == Row) {
					uint Left = ((Table.TableChild)(this.datatablePhones[w])).LeftAttach;
					uint Right = ((Table.TableChild)(this.datatablePhones[w])).RightAttach;
					datatablePhones.Remove(w);
					if(w.GetType() == typeof(yListComboBox))
						datatablePhones.Attach(w, Left, Right, Row - 1, Row, AttachOptions.Fill | AttachOptions.Expand, (AttachOptions)0, (uint)0, (uint)0);
					else
						datatablePhones.Attach(w, Left, Right, Row - 1, Row, (AttachOptions)0, (AttachOptions)0, (uint)0, (uint)0);
				}
		}

		private void SetEditable()
		{
			foreach(Widget w in datatablePhones.Children) {
				if(w is Entry)
					(w as Entry).IsEditable = !IsReadOnly;
				else if(w is Button)
					(w as Button).Visible = !IsReadOnly;
			}
			if(PhonesList != null)
				buttonAdd.Visible = !isReadOnly;
		}

		private void CleanList()
		{
			while(PhonesList.Count > 0) {
				PhonesList.RemoveAt(0);
			}
		}

		/// <summary>
		/// Необходимо выполнить перед сохранением или в геттере HasChanges
		/// </summary>
		public void RemoveEmpty()
		{
			PhonesList.Where(p => p.DigitsNumber.Length < ContactParametersProvider.Instance.MinSavePhoneLength)
				.ToList().ForEach(p => PhonesList.Remove(p));
		}
	}
}