//using DTO.HumanResource;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LINK_UI.App_start
//{
//    public class CustomModelBinder : IModelBinder
//    {
//        public Task BindModelAsync(ModelBindingContext bindingContext)
//        {
//            throw new NotImplementedException();
//        }


//    }

//    public class CustomModelBinderProvider : IModelBinderProvider
//    {
//        public IModelBinder GetBinder(ModelBinderProviderContext context)
//        {
//            if (context == null)
//            {
//                throw new ArgumentNullException(nameof(context));
//            }

//            if (context.Metadata.ModelType == typeof(StaffDetails))
//            {
//                return new BinderTypeModelBinder(typeof(CustomModelBinder));
//            }

//            return null;
//        }
//    }


//}
