using DialogueSystem.Abstract;
using DialogueSystem.Nodes;
using DialogueSystem.Window;

namespace DialogueSystem.Generators
{
    internal class ClassGenerator : GHelper
    {
        DSClassInfo<DSGraphView> dsGrathViewClass = null;
        ClassDrawer mainClassDrawer;

        internal ClassGenerator(DSGraphView grathView, string className)
        {
            dsGrathViewClass = new(grathView);
            dsGrathViewClass.ClassName = className;
            mainClassDrawer = new();
        }

        internal void Initialize()
        {
            dsGrathViewClass.Initialize();

            foreach (IDataHolder dataHolder in dsGrathViewClass.DataHolders)
            {
                switch(dataHolder)
                {
                    case BasePrimitiveNode primitive:
                        DSClassInfo<BasePrimitiveNode> classInfoPrimitive = new(primitive);                        
                        dsGrathViewClass.RegisterInnerClass(classInfoPrimitive);
                        classInfoPrimitive.Initialize();

                        break;

                    case BaseOperationNode operation:
                        DSClassInfo<BaseOperationNode> classInfoOperation = new(operation);
                        dsGrathViewClass.RegisterInnerClass(classInfoOperation);
                        classInfoOperation.Initialize();
                        break;

                    case BaseConvertNode convert:
                        DSClassInfo<BaseOperationNode> classInfoConvert = new(convert);
                        dsGrathViewClass.RegisterInnerClass(classInfoConvert);
                        classInfoConvert.Initialize();
                        break;

                    case BaseLogicNode logic:
                        DSClassInfo<BaseOperationNode> classInfoLogic = new(logic);
                        dsGrathViewClass.RegisterInnerClass(classInfoLogic);
                        classInfoLogic.Initialize();
                        break;

                    case BaseLetterNode letter:
                        DSClassInfo<BaseOperationNode> classInfoLetter = new(letter);
                        dsGrathViewClass.RegisterInnerClass(classInfoLetter);
                        classInfoLetter.Initialize();
                        break;

                    case BaseDialogueNode dialogue:
                        DSClassInfo<BaseOperationNode> classInfoDialogue = new(dialogue);
                        dsGrathViewClass.RegisterInnerClass(classInfoDialogue);
                        classInfoDialogue.Initialize();
                        break;

                }
            }
        }

        internal ClassDrawer GetDrawer()
        {
            mainClassDrawer.ClassDeclaration(dsGrathViewClass.ClassName, Attribute.None, Visibility.@public, new System.Type[] { typeof(DialogueDisposer) });
            mainClassDrawer.
            foreach (var item in dsGrathViewClass.InnerClassInfo)
            {
                ClassDrawer innerClass = new();
            }

            return mainClassDrawer;
        }
    }
}
