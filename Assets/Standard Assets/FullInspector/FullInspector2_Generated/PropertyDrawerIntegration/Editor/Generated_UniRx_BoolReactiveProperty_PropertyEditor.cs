using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(UniRx.BoolReactiveProperty))]
    public class Generated_UniRx_BoolReactiveProperty_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_UniRx_BoolReactiveProperty_MonoBehaviourStorage, UniRx.BoolReactiveProperty> {
        public override bool CanEdit(Type type) {
            return typeof(UniRx.BoolReactiveProperty).IsAssignableFrom(type);
        }
    }
}
