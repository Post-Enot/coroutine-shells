using System;
using System.Collections;
using UnityEngine;

namespace CoroutineShells
{
    /// <summary>
    /// �����-��������, ��������������� ������ �������������� � ���������� ��� ����������.
    /// </summary>
    public sealed class UniqueCoroutine
    {
        /// <summary>
        /// ������ ������-��������, ������� � �������������.
        /// </summary>
        /// <param name="performer">MonoBehaviour-������, � ������� �������� ����� ����������� ��������.
        /// ���������� ��� �������� ����� ������� ������� � ��������� �������� ��������.</param>
        /// <param name="getRoutine">�������-����� ��������.</param>
        public UniqueCoroutine(MonoBehaviour performer, Func<IEnumerator> getRoutine)
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

        private readonly Func<IEnumerator> _getRoutine;
        private Coroutine _coroutine;

        /// <summary>
        /// ��������� ��������, ���� ��� �� ����� �� ���� ��������. � ��������� ������ �������� ������������.
        /// </summary>
        public void Start()
        {
            if (!IsPerformed)
            {
                _coroutine = Performer.StartCoroutine(Routine());
            }
        }

        /// <summary>
        /// ��������� ��������.
        /// ���� ��� �� ����� ���� ��������, �� ������������� ��������� � ����������, ����� ���� ��������� �����.
        /// </summary>
        public void StartAnyway()
        {
            if (IsPerformed)
            {
                Performer.StopCoroutine(_coroutine);
            }
            _coroutine = Performer.StartCoroutine(Routine());
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

        private IEnumerator Routine()
        {
            IsPerformed = true;
            yield return _getRoutine();
            IsPerformed = false;
        }
    }
}
