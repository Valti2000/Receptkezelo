using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Repositories;
using System;
using System.Threading.Tasks;

namespace Recept.Services
{
    public class UnitOfWork : IDisposable
    {
        private readonly ReceptekContext _dbContext;
        private readonly Lazy<IReceptRepository> _receptRepository;
        private readonly Lazy<IReceptHozzavaloRepository> _receptHozzavaloRepository;
        private readonly Lazy<IAllergenRepository> _allergenRepository;
        private readonly Lazy<IAlapanyagRepository> _alapanyagRepository;
        private readonly Lazy<ICsoportRepository> _csoportRepository;
        private readonly Lazy<IHozzavaloRepository> _hozzavaloRepository;
        private readonly Lazy<IKategoriaRepository> _kategoriaRepository;
        private readonly Lazy<IAlapanyagAllergenRepository> _alapanyagAllergenRepository;

        public UnitOfWork(ReceptekContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _alapanyagAllergenRepository = new Lazy<IAlapanyagAllergenRepository>(() => new AlapanyagAllergenRepository(_dbContext)); 
            _receptHozzavaloRepository = new Lazy<IReceptHozzavaloRepository>(() => new ReceptHozzavaloRepository(_dbContext));
            _allergenRepository = new Lazy<IAllergenRepository>(() => new AllergenRepository(_dbContext));
            _receptRepository = new Lazy<IReceptRepository>(() => new ReceptRepository(_dbContext, _receptHozzavaloRepository.Value, _allergenRepository.Value));
            _alapanyagRepository = new Lazy<IAlapanyagRepository>(() => new AlapanyagRepository(_dbContext, _alapanyagAllergenRepository.Value,  _allergenRepository.Value));
            _csoportRepository = new Lazy<ICsoportRepository>(() => new CsoportRepository(_dbContext));
            _hozzavaloRepository = new Lazy<IHozzavaloRepository>(() => new HozzavaloRepository(_dbContext));
            _kategoriaRepository = new Lazy<IKategoriaRepository>(() => new KategoriaRepository(_dbContext));
        }

        public IReceptRepository ReceptRepository => _receptRepository.Value ?? throw new NullReferenceException(nameof(_receptRepository));
        public IReceptHozzavaloRepository ReceptHozzavaloRepository => _receptHozzavaloRepository.Value ?? throw new NullReferenceException(nameof(_receptHozzavaloRepository));
        public IAllergenRepository AllergenRepository => _allergenRepository.Value ?? throw new NullReferenceException(nameof(_allergenRepository));
        public IAlapanyagRepository AlapanyagRepository => _alapanyagRepository.Value ?? throw new NullReferenceException(nameof(_alapanyagRepository));
        public ICsoportRepository CsoportRepository => _csoportRepository.Value ?? throw new NullReferenceException(nameof(_csoportRepository));
        public IHozzavaloRepository HozzavaloRepository => _hozzavaloRepository.Value ?? throw new NullReferenceException(nameof(_hozzavaloRepository));
        public IKategoriaRepository KategoriaRepository => _kategoriaRepository.Value ?? throw new NullReferenceException(nameof(_kategoriaRepository));
        public IAlapanyagAllergenRepository AlapanyagAllergenRepository => _alapanyagAllergenRepository.Value ?? throw new NullReferenceException(nameof(_alapanyagAllergenRepository));

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
