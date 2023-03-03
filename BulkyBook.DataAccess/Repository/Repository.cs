﻿using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository {
	public class Repository<T> : IRepository<T> where T : class {

		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext db) {
			_db = db;
			this.dbSet = _db.Set<T>();
		}

		public void Add(T entity) {
			dbSet.Add(entity);
		}
		// includeProp - "Category,CoverType"
		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null) {
			IQueryable<T> query = dbSet;
			if(filter != null) query = query.Where(filter);

            if (includeProperties != null) { 
				foreach(var inclueProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
					query = query.Include(inclueProp);
				}
			}
			return query.ToList();
		}

		public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true) {
			IQueryable<T> query = dbSet;
			if (tracked) {
				query = dbSet;
			} else {
				query = dbSet.AsNoTracking();// avoids that changes are applied directly when updating object properties
			}
			query = query.Where(filter);
			if (includeProperties != null) {
				foreach (var inclueProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
					query = query.Include(inclueProp);
				}
			}
			return query.FirstOrDefault();
		}

		public void Remove(T entity) {
			dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entity) {
			dbSet.RemoveRange(entity);
		}
	}
}
