using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace Umbraco9Membership.Services
{
    public class DataTypeValueService : IDataTypeValueService
    {
        private readonly IDataTypeService _dataTypeService;

        public DataTypeValueService(IDataTypeService dataTypeService)
        {
            _dataTypeService = dataTypeService;
        }

        public IEnumerable<SelectListItem> GetItemsFromValueListDataType(string dataTypeName, string[] selectedValues)
        {
            IEnumerable<SelectListItem> items = null;

            var dataTypeConfig =
                (ValueListConfiguration)_dataTypeService.GetDataType(dataTypeName).Configuration;


            if (dataTypeConfig?.Items != null && dataTypeConfig.Items.Any())
            {
                items =
                    dataTypeConfig.Items.Select(x => new SelectListItem()
                    {
                        Text = x.Value,
                        Value = x.Value,
                        Selected = selectedValues != null && selectedValues.Contains(x.Value)
                    });
            }

            return items;
        }
    }
}
