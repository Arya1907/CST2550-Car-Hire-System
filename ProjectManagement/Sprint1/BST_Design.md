# Binary Search Tree (BST) Design
Inspire Car Hire Management System

---

## 1. Introduction

The Inspire Car Hire Management System requires an efficient way to store, search, insert, and remove car records. 

To achieve this, a **custom Binary Search Tree (BST)** will be implemented instead of using built-in data structures.

The BST will store **Car objects**, using PricePerDay as the key for ordering to support efficient price-range searching.

This ensures efficient searching and management of vehicles in the system.

## 2. Why Binary Search Tree?

The BST was chosen because:

- It provides faster searching compared to linear lists.
- It allows efficient insertion and deletion.
- It supports ordered data storage.
- It demonstrates understanding of custom data structures (coursework requirement).

Time Complexity (Average Case):
- Search: O(log n)
- Insert: O(log n)
- Delete: O(log n)

Worst Case (Unbalanced Tree):
- Search: O(n)
- Insert: O(n)
- Delete: O(n)

The worst case occurs when the tree becomes skewed 
(e.g., inserting already sorted data), causing it to behave like a linked list.

---

## 3. Structure of BST Node

Each node in the Binary Search Tree will contain:

- Car object
- Reference to Left child
- Reference to Right child

Conceptual Structure:

Node
    Car Data
    Left → Node
    Right → Node


## 4. Key Used for Comparison

The BST will use:

PricePerDay (decimal)

Rules:
- If new PricePerDay < current node PricePerDay → go LEFT
- If new PricePerDay >= current node PricePerDay → go RIGHT
- Cars with equal prices are placed in the right subtree

---

## 5. Operations of the BST

### 5.1 Insert Operation

Purpose:
Used by Admin to add new cars to the system.

Logic:
- If tree is empty → new node becomes root.
- If PricePerDay is smaller → insert in left subtree.
- If PricePerDay is greater or equal → insert in right subtree.

Pseudo Code:

FUNCTION Insert(node, car)

    IF node IS NULL
        CREATE new node with car
        RETURN new node

    IF car.PricePerDay < node.Car.PricePerDay
        node.Left = Insert(node.Left, car)

    ELSE
        node.Right = Insert(node.Right, car)

    RETURN node

---

### 5.2 Search Operation

Purpose:
Used by:
- Customer to search for cars within a price range
- Admin to verify records

Logic:
- Compare node PricePerDay against min and max bounds
- Traverse left if node price is greater than min (smaller prices may exist)
- Traverse right if node price is less than max (larger prices may exist)
- Collect matching nodes into results

Pseudo Code:

FUNCTION SearchByPriceRange(node, min, max, results)

    IF node IS NULL
        RETURN

    IF node.Car.PricePerDay > min
        SearchByPriceRange(node.Left, min, max, results)

    IF node.Car.PricePerDay >= min AND node.Car.PricePerDay <= max
        results.Add(node.Car)

    IF node.Car.PricePerDay < max
        SearchByPriceRange(node.Right, min, max, results)

---

### 5.3 Delete Operation

Purpose:
Used by Admin to remove a car from the system.

Three Cases:

1. Node has no children → simply remove.
2. Node has one child → replace with child.
3. Node has two children → replace with inorder successor.

Pseudo Code (Simplified):

FUNCTION Delete(node, car)

    IF node IS NULL
        RETURN NULL

    IF car.PricePerDay < node.Car.PricePerDay
        node.Left = Delete(node.Left, car)

    ELSE IF car.PricePerDay > node.Car.PricePerDay
        node.Right = Delete(node.Right, car)

    ELSE
        IF node.Car.Id != car.Id
            node.Right = Delete(node.Right, car)
            RETURN node

        IF node has no children
            RETURN NULL

        IF node has one child
            RETURN child node

        IF node has two children
            successor = FindMin(node.Right)
            node.Car = successor.Car
            node.Right = Delete(node.Right, successor.Car)

    RETURN node

---

## 6. Traversal Method

To display cars (View Cars option), the system will use:

Inorder Traversal

Why?
Because it prints cars in ascending order of PricePerDay.

Pseudo Code:

FUNCTION Inorder(node)

    IF node IS NOT NULL
        Inorder(node.Left)
        PRINT node.Car
        Inorder(node.Right)

---

## 7. Integration with System Roles

Admin:
- Insert Car
- Delete Car
- View All Cars (Inorder Traversal)

Staff:
- Search Car
- Mark car as unavailable (IsAvailable = false)

Customer:
- Search Car
- View Available Cars

The BST ensures efficient access for all roles.

---

## 8. Limitations

If the BST becomes unbalanced (e.g., sorted insertion),
performance may degrade to O(n).

Future Improvement:
Use a self-balancing tree (AVL or Red-Black Tree).

---

## 9. Conclusion

The Binary Search Tree is a suitable data structure for managing car records in the Inspire Car Hire Management System.

It provides:
- Efficient searching
- Structured storage
- Logical organisation
- Academic demonstration of custom data structure implementation

This design satisfies the coursework requirement for implementing and analysing a custom data structure.
