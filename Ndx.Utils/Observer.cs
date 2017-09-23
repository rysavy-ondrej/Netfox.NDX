using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Utils
{
    public class Observer<T> : IObserver<T>
    {
        Action<T> m_onNext;
        Action m_onCompleted;
        Action<Exception> m_onError;
        public Observer(Action<T> onNext, Action onCompleted = null, Action<Exception> onError = null)
        {
            m_onNext = onNext;
            m_onCompleted = onCompleted;
            m_onError = onError;
        }
        public void OnCompleted()
        {
            m_onCompleted?.Invoke();
        }

        public void OnError(Exception error)
        {
            m_onError?.Invoke(error);
        }

        public void OnNext(T value)
        {
            m_onNext?.Invoke(value);
        }
    }
}
