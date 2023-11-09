using Microsoft.AspNetCore.Components;
using Radzen;
using System.Reflection;

namespace BlazorWASMOnionMessenger.Client.Shared
{
    public abstract class BaseGrid<TEntity> : ComponentBase
    {
        [SupplyParameterFromQuery]
        [Parameter]
        public int Page { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public int PageSize { get; set; }
        [SupplyParameterFromQuery]
        [Parameter]
        public string OrderBy { get; set; } = String.Empty;
        [SupplyParameterFromQuery]
        [Parameter]
        public bool OrderType { get; set; } = false;
        [SupplyParameterFromQuery]
        [Parameter]
        public string Search { get; set; } = String.Empty;

        protected List<TEntity> Items { get; set; } = new List<TEntity>();
        protected List<string> OrderProps { get; } = new List<string>();
        protected List<int> PageSizeOptions { get; } = new List<int> { 2, 5, 10, 20, 30 };

        protected int Total { get; set; }
        protected bool isLoading = false;

        protected string ConstructRouteTemplate(string route)
        {
            return $"{route}?page={Page}&pageSize={PageSize}&orderBy={OrderBy}&orderType={OrderType}&search={Search}";
        }
        protected abstract Task Navigate();
        protected async void OnPageChange(PagerEventArgs args)
        {
            Page = args.PageIndex + 1;
            await Navigate();
        }

        protected async void OnCheckBoxChange(bool value) => await Navigate();
        protected void PopulateOrderProps()
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                OrderProps.Add(property.Name);
            }
        }
    }
}
