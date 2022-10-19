using System;
using System.Collections;
using UnityEngine;

namespace CoroutineShells
{
    /// <summary>
    /// �����-��������, ��������������� ������ �������������� � ����������, ������������ 1 ��������.
    /// </summary>
    public sealed class UniqueCoroutine<T>
    {
        /// <summary>
        /// ������ ������-��������, ������� � �������������.
        /// </summary>
        /// <param name="performer">MonoBehaviour-������, � ������� �������� ����� ����������� ��������.
        /// ���������� ��� �������� ����� ������� ������� � ��������� �������� ��������.</param>
        /// <param name="getRoutine">�������-����� ��������.</param>
        public UniqueCoroutine(MonoBehaviour performer, Func<T, IEnumerator> getRoutine)
        {
            Performer = performer;
            _getRoutine = getRoutine;
        }

        /// <summary>
        /// �������� �� �������� � ������� ������.
        /// </summary>
        public bool IsPerformed { get; private set; }
        /// <summary>
        /// ������ �� ������, � ������� �������� ����������� ��������.
        /// </summary>
        public readonly MonoBehaviour Performer;

        private readonly Func<T, IEnumerator> _getRoutine;
        private Coroutine _coroutine;

        /// <summary>
        /// ��������� ��������, ���� ��� �� ����� �� ���� ��������. � ��������� ������ �������� ������������.
        /// </summary>
        /// <param name="arg">��������, ����������� ���������.</param>
        public void Start(T arg)
        {
            if (!IsPerformed)
            {
                _coroutine = Performer.StartCoroutine(Routine(arg));
            }
        }

        /// <summary>
        /// ��������� ��������.
        /// ���� ��� �� ����� ���� ��������, �� ������������� ��������� � ����������, ����� ���� ��������� �����.
        /// </summary>
        /// <param name="arg">��������, ����������� ���������.</param>
        public void StartAnyway(T arg)
        {
            if (IsPerformed)
            {
                Performer.StopCoroutine(_coroutine);
            }
            _coroutine = Performer.StartCoroutine(Routine(arg));
        }

        /// <summary>
        /// ������������� ��������, ���� ��� �� ����� ���� ��������. � ��������� ������ �������� ������������.
        /// </summary>
        public void Stop()
        {
            if (IsPerformed)
            {
                Performer.StopCoroutine(_coroutine);
                IsPerformed = false;
            }
        }

        private IEnumerator Routine(T arg)
        {
            IsPerformed = true;
            yield return _getRoutine(arg);
            IsPerformed = false;
        }
    }
}
