using System;
using System.Collections;
using UnityEngine;

namespace CoroutineShells
{
    /// <summary>
    /// �����-��������, ��������������� ������ �������������� � ����������, ������������ 2 ���������.
    /// </summary>
    public sealed class UniqueCoroutine<T1, T2>
    {
        /// <summary>
        /// ������ ������-��������, ������� � �������������.
        /// </summary>
        /// <param name="performer">MonoBehaviour-������, � ������� �������� ����� ����������� ��������.
        /// ���������� ��� �������� ����� ������� ������� � ��������� �������� ��������.</param>
        /// <param name="getRoutine">�������-����� ��������.</param>
        public UniqueCoroutine(MonoBehaviour performer, Func<T1, T2, IEnumerator> getRoutine)
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

        private readonly Func<T1, T2, IEnumerator> _getRoutine;
        private Coroutine _coroutine;

        /// <summary>
        /// ��������� ��������, ���� ��� �� ����� �� ���� ��������. � ��������� ������ �������� ������������.
        /// </summary>
        /// <param name="arg1">������ ��������, ����������� ���������.</param>
        /// <param name="arg2">������ ��������, ����������� ���������.</param>
        public void Start(T1 arg1, T2 arg2)
        {
            if (!IsPerformed)
            {
                _coroutine = Performer.StartCoroutine(Routine(arg1, arg2));
            }
        }

        /// <summary>
        /// ��������� ��������.
        /// ���� ��� �� ����� ���� ��������, �� ������������� ��������� � ����������, ����� ���� ��������� �����.
        /// </summary>
        /// <param name="arg1">������ ��������, ����������� ���������.</param>
        /// <param name="arg2">������ ��������, ����������� ���������.</param>
        public void StartAnyway(T1 arg1, T2 arg2)
        {
            if (IsPerformed)
            {
                Performer.StopCoroutine(_coroutine);
            }
            _coroutine = Performer.StartCoroutine(Routine(arg1, arg2));
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

        private IEnumerator Routine(T1 arg1, T2 arg2)
        {
            IsPerformed = true;
            yield return _getRoutine(arg1, arg2);
            IsPerformed = false;
        }
    }
}
