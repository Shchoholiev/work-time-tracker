namespace TimeTracker.Application.Paging
{
    public class PagedList<T> : List<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public PagedList() { }

        public PagedList(IEnumerable<T> items, PageParameters pageParameters, int totalItems)
        {
            PageNumber = pageParameters.PageNumber;
            PageSize = pageParameters.PageSize;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageParameters.PageSize);

            AddRange(items);
        }
    }
}
