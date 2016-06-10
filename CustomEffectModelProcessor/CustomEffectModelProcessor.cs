using System;
using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace CustomEffectModelProcessor
{
    [ContentProcessor(DisplayName = "CustomEffectModelProcessor")]
    public class CustomEffectModelProcessor : ModelProcessor
    {
        private ContentIdentity _identity;

        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            _identity = input.Identity;
            return base.Process(input, context);
        }

        protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context)
        {
            EffectMaterialContent myMaterial = new EffectMaterialContent();
            myMaterial.Effect = new ExternalReference<EffectContent>("InstancedModel.fx", _identity);

            context.Logger.LogWarning("help", _identity, 
                $"MATERIAL [ Name: {material.Name}, Textures: {material.Textures.Keys.Aggregate((s1, s2) => $"{s1}, {s2}")}]");

            // Copy the texture setting across from the original material.
            BasicMaterialContent basicMaterial = material as BasicMaterialContent;

            if (basicMaterial?.Texture != null)
            {
                myMaterial.Textures.Add("Texture", basicMaterial.Texture);
                context.Logger.LogWarning("help", _identity, 
                    $"MATERIAL [ Name: {myMaterial.Name}, Textures: {myMaterial.Textures.Keys.Aggregate((s1, s2) => $"{s1}, {s2}")}]");
            }



            return base.ConvertMaterial(myMaterial, context);
        }
    }
}