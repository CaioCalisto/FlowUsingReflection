using System.Reflection;
using UpdateFlow.Products;
using UpdateFlow.Transformation.Bullets;
using UpdateFlow.Transformation.Specials;

namespace UpdateFlow;

public class SecurityFlow
{
    public async Task Execute(Product product)
    {
        string finalProduct = product.Name;
        
        // first step: apply bullet transformation
        var bullets = GetProcess<BulletCreator>(product.Name);
        foreach (var bullet in bullets)
        {
            finalProduct = finalProduct + " " + await bullet.ApplyAsync();
        }

        // second step: apply special transformation
        var specials = GetProcess<SpecialCreator>(product.Name);
        foreach (var special in specials)
        {
            finalProduct = finalProduct + " - " + await special.ApplyAsync();
        }

        // third step: save in some external service

    }

    private IEnumerable<T> GetProcess<T>(string productType)
    {
        List<T> operators = new List<T>();

        var allOperators = Assembly.GetAssembly(typeof(T)).GetTypes()
            .Where(t => (typeof(T)).IsAssignableFrom(t) 
                        && t.IsClass 
                        && t.IsInterface == false 
                        && t.IsAbstract == false
                        && GetProductAttribute(t).Any(a => a.GetProductType() == productType));
        
        foreach (Type type in allOperators)
        {
            operators.Add((T)Activator.CreateInstance(type));
        }
        
        return operators;
    }

    private IEnumerable<ProductAttribute> GetProductAttribute(Type type)
    {
        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(type);
        return attrs.Where(a => a is ProductAttribute).Select(a => a as ProductAttribute);
    }
}