 using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
using ProcMgt_Reference_Core;
using ProcMgt_Reference_Core.GenericRepoInter;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Interfaces;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Implementations

{
    public class AuthService : IAuthServices
    {
        private IGenericRepo<User> _repository = null;
        private IUnitOfWorks _unitOfWork;

        private IGenericRepo<Menu> _menurepository = null;
      
        private IGenericRepo<RoleMenu> _rolemenurepository = null;

        private IGenericRepo<Module> _modulerepository = null;

        private IGenericRepo<ModuleMenu> _modulemenurepository = null;


        public AuthService(IGenericRepo<User> repository, IUnitOfWorks unitfwork, IGenericRepo<Menu> menurepository, IGenericRepo<User> userrepository, IGenericRepo<RoleMenu> rolemenurepository, IGenericRepo<Module> modulerepository, IGenericRepo<ModuleMenu> modulemenurepository)
        {
            this._repository = repository;
            this._unitOfWork = unitfwork;
             
            this._menurepository = menurepository;
            
            this._rolemenurepository = rolemenurepository;
            this._modulerepository = modulerepository;
            this._modulemenurepository = modulemenurepository;


         

        }

        //private readonly AppSettings _appSettings;

        //public AuthService(IOptions<AppSettings> appSettings)
        //{
        //    _appSettings = appSettings.Value;
        //}


        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        List<User> _users = new List<User>();

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _repository.GetAll();
            // return users without passwords
            //return _users.Select(x => {
            //    x.Password = null;
            //    return x;
            //});
        }

        public async Task<AuthenticatorResource> Authenticate(string username, string password)
        {


            AuthenticatorResource authUser = new AuthenticatorResource();

            try {
                var userList = await _repository.GetAll();
                User user = await Task.Run(() => userList.SingleOrDefault(x => x.UserName == username && x.Password == password));

                if (user == null)
                {
                    return null;
                }


                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                //var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var menuIds = (await _rolemenurepository.GetAll()).ToList();
                var modulemenuIds = (await _modulemenurepository.GetAll()).ToList();
                //var moduleIds = (await _modulerepository.GetAll()).ToList();

                IEnumerable<RoleMenuResource> menuIdslst = (await _rolemenurepository.GetAll()).Where(a => a.UserRoleId == user.UserRoleId).Select(b => new RoleMenuResource()
                {
                    MenuID = b.MenuId

                }).ToList();

               

                IEnumerable<MenuResource> authmenuList = (await _menurepository.GetAll()).Select(b => new MenuResource()
                {
                    MenuID = b.MenuId,
                    MenuName = b.MenuName,
                    MenuIDHTML = b.MenuIdhtml,
                    Icon = b.Icon,
                    URL = b.Url

                }).Where(a => menuIdslst.Select(c => c.MenuID).Contains(a.MenuID)).ToList();

                IEnumerable<ModuleResource> authmoduleList = (await _modulerepository.GetAll()).Select(e => new ModuleResource()
                {
                    ModuleID = e.ModuleId,
                    ModuleName = e.ModuleName


                }).ToList();



                IEnumerable<ModuleMenuResource>moduleMenuList = (await _modulemenurepository.GetAll()).Select(d => new ModuleMenuResource()
                {
                    ModuleID = d.ModuleId,
                    ModuleMenuID = d.ModuleMenuId,

                    MenuID = d.MenuId,

                    ModuleName = _modulerepository.GetByIdAsync(d.ModuleId).Result.ModuleName.ToString(),
                    MenuName = _menurepository.GetByIdAsync(d.MenuId).Result.MenuName.ToString(),
                    MenuIDHTML = _menurepository.GetByIdAsync(d.MenuId).Result.MenuIdhtml.ToString(),
                    Icon = _menurepository.GetByIdAsync(d.MenuId).Result.Icon.ToString(),
                    URL = _menurepository.GetByIdAsync(d.MenuId).Result.Url.ToString(),

                }).Where(b => menuIdslst.Select(e => e.MenuID).Contains(b.MenuID)).ToList();

              //  moduleMenuList = moduleMenuList.GroupBy(x => new { x.ModuleID }).Select(g => g.FirstOrDefault()).ToList();

                

                //model.Links = db.Links.GroupBy(l => l.category_name)
                //       .Select(g => g.FirstOrDefault())
                //       .ToList();

                authUser.UserDetails = user;
                authUser.MenuDetails = authmenuList;
                authUser.ModuleMenuDetails = moduleMenuList;
                authUser.ModuleDetails = authmoduleList;







                if (userList == null)
                {
                    return null;
                }
                return authUser;

            }
            catch(Exception ex)
            {
                return null;
            }

          

        }
    }
}
