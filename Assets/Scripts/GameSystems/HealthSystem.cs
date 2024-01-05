using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private List<Heart> heartList;
    public HealthSystem(int heartAmount)
    {
        heartList = new List<Heart>();
        for(int i = 0; i  < heartAmount; i++)
        {
            Heart heart = new Heart(4);
            heartList.Add(heart);
        }
    }

    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    public class Heart
    {
        private int fragment;

        public Heart(int fragments)
        {
            this.fragment = fragments;
        }

        public int getFragmentAmount()
        {
            return this.fragment;
        }
    }

}
