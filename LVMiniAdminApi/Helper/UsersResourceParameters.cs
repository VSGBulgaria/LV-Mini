namespace LVMiniAdminApi.Helper
{
    public class UsersResourceParameters
    {
        private const int maxPageSize = 20;
        private const int defaultPagesNumbers = 1;
        private const int defaultPageSize = 10;

        private int _pageSize = defaultPageSize;

        public int PageNumber { get; set; } = defaultPagesNumbers;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value > maxPageSize)
                {
                    _pageSize = maxPageSize;
                }
                else
                {
                    _pageSize = value;
                }

            }

        }
    }
}
