using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
	public int maxSize;
	public List<Item> contents = new List<Item>();

	Inventory(int max){
		maxSize = max;
		for(int i=0;i<maxSize;i++){
			contents.Add(null);
		}
	}

	public Item Add(Item item){
		Item output = null;
		List<int> stackIndices = FindById(item.itemId);
		for(int i=0;i<stackIndices.Count; i++){
			Get(i).stackCount += item.stackCount;
			if(Get(i).stackCount > item.maxStackCount){
				item.stackCount = Get(i).stackCount - item.maxStackCount;
				Get(i).stackCount = item.maxStackCount;
			}else{
				item.stackCount = 0;
			}
			if(item.stackCount == 0){
				break;
			}
		}
		if(item.stackCount != 0){
			output = item;
			for(int i=0;i<maxSize;i++){
				if(Get(i) == null){
					contents[i] = item;
					output = null;
					break;
				}
			}
		}
		return output;
	}

	public Item Add(Item item, int index){
		Item output;
		if(contents[index] != null){
			if(contents[index].itemId == item.itemId){
				contents[index].stackCount += item.stackCount;
				if(contents[index].stackCount > contents[index].maxStackCount){
					output = new Item();
					output.itemId = item.itemId;
					output.stackCount = contents[index].stackCount - contents[index].maxStackCount;
					contents[index].stackCount = contents[index].maxStackCount;
				}else{
					output = null;
				}
			}else{
				output = contents[index];
				contents[index] = item;
			}
		}else{
			contents[index] = item;
			output = null;
		}
		return output;
	}

	public Item Get(int index){
		return contents[index];
	}

	List<int> FindById(int id){
		List<int> output = new List<int>();
		for(int i=0;i<maxSize;i++){
			if(Get(i).itemId == id){
				output.Add(i);
			}
		}
		return output;
	}
}

public class Item {
	public int itemId = 0;
	public int stackCount = 1;
	public int maxStackCount = 20;
}