using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.LessonFeature.OptionFeature
{
    public interface IOptionRepositoryFactory
    {
    IOptionRepository CreateRepository(OptionType optionType);
    }

    public enum OptionType
    {
        Text = 1,
        Image
    }
    public class OptionRepositoryFactory : IOptionRepositoryFactory
    {
        public IOptionRepository CreateRepository(OptionType optionType)
        {
            switch (optionType)
            {
                case OptionType.Text:
                    return new TextOptionRepository();
                default:
                    return new TextOptionRepository();
            }
        }

  
    }
}
