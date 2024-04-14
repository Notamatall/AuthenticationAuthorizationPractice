using Microsoft.Extensions.Options;

namespace API.CustomServices.KnightService
{
    public class KnightFactory : IKnightFactory
    {
        private readonly KnightOptions options;
        public KnightFactory(IOptions<KnightOptions> options)
        {
            this.options = options.Value;
        }
        public Knight CreateKnight()
        {
            return new Knight()
            {
                Armor = options.Armor,
                HP = options.HP,
            };
        }
    }


}
