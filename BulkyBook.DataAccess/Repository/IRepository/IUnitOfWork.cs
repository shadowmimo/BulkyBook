using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository {
	public interface IUnitOfWork {
		ICategoryRepository Category{ get; }
		ICoverTypeRepository CoverType { get; }

		IProductRepository Product { get; }

		ICompanyRepository Company { get; }

        IShoppingCartRepository ShoopingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }

        void Save();

	}
}
